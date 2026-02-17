using UnityEngine;
using DG.Tweening; // Necessario per DOTween

public class NpcProximityCaller : MonoBehaviour
{
    [Header("Risorse Dirette")]
    [SerializeField] private AudioClip _voiceClip; 
    [SerializeField] private SubtitleData _subtitle; 
    [SerializeField] private AppEventData _onSubtitleShow; 

    [Header("Riferimenti Fisici")]
    [SerializeField] private AudioSource _audioSource; 
    private Animator _animator;
    private NpcBrain _brain;

    [Header("Parametri Sensore")]
    [SerializeField] private float detectionRange = 7.0f; 
    [SerializeField] private float cooldownTime = 10.0f; 
    
    [Header("Animazione")]
    [SerializeField] private string callTriggerName = "call"; 

    [Header("Sincronizzazione")]
    [SerializeField] private float _audioDelay = 1.0f; // <--- Secondi di ritardo prima dell'audio

    [Header("Rotazione e Ritorno")]
    [SerializeField] private bool _shouldRotate = true;
    [SerializeField] private float _rotationSpeed = 5.0f; // Aumentata da .0f per permettere il movimento
    [SerializeField] private float _returnSpeed = 1.0f; 
    [SerializeField] private bool _onlyRotateWhileTalking = false;

    private Transform _playerTransform;
    private Quaternion _initialRotation; 
    private float _lastCallTime = -100f;
    private bool _playerInRange = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _brain = GetComponent<NpcBrain>(); //
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _initialRotation = transform.rotation;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    void Update()
    {
        if (_playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        if (distance <= detectionRange && !_playerInRange && Time.time >= _lastCallTime + cooldownTime)
        {
            if (_brain != null && _brain.currentState == NpcState.Talk) return; //
            ExecuteCall();
        }
        else if (distance > detectionRange)
        {
            _playerInRange = false;
        }

        if (_shouldRotate)
        {
            if (_playerInRange) HandleRotationTowardsPlayer();
            //else ReturnToOriginalPosition();
        }
    }

    private void HandleRotationTowardsPlayer()
    {
        if (_onlyRotateWhileTalking && _audioSource != null && !_audioSource.isPlaying) 
        {
            //ReturnToOriginalPosition();
            return;
        }

        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }

    private void ReturnToOriginalPosition()
    {
        if (transform.rotation != _initialRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, Time.deltaTime * _returnSpeed);
        }
    }

    private void ExecuteCall()
    {
        _playerInRange = true;
        _lastCallTime = Time.time;

        // 1. L'animazione parte subito
        if (_animator != null) _animator.SetTrigger(callTriggerName);

        // 2. Audio e Testo partono dopo il ritardo impostato usando DOTween
        DOVirtual.DelayedCall(_audioDelay, () => 
        {
            if (_audioSource != null && _voiceClip != null)
            {
                _audioSource.clip = _voiceClip;
                _audioSource.Play(); 
            }

            if (_onSubtitleShow != null && _subtitle != null)
            {
                SubtitleDataTimeReference st; 
                st.subtitleData = _subtitle;
                st.audioSource = _audioSource;
                st.playableDirector = null;
                _onSubtitleShow.RaiseWithParam(st); 
            }
        });
    }
}