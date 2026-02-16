using UnityEngine;

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

    [Header("Rotazione (Inspector)")]
    [SerializeField] private bool _shouldRotate = true;
    [SerializeField] private float _rotationSpeed = 5.0f;
    [Tooltip("Se attivo, ruota verso il player solo quando l'audio sta andando")]
    [SerializeField] private bool _onlyRotateWhileTalking = false;

    private Transform _playerTransform;
    private float _lastCallTime = -100f;
    private bool _playerInRange = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _brain = GetComponent<NpcBrain>();
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    void Update()
    {
        if (_playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        // --- Logica di Attivazione ---
        if (distance <= detectionRange && !_playerInRange && Time.time >= _lastCallTime + cooldownTime)
        {
            if (_brain != null && _brain.currentState == NpcState.Talk) return;
            ExecuteCall();
        }
        else if (distance > detectionRange)
        {
            _playerInRange = false;
        }

        // --- Logica di Rotazione ---
        if (_shouldRotate && _playerInRange)
        {
            HandleRotation();
        }
    }

    private void HandleRotation()
    {
        // Se impostato, ruota solo se l'audio è in riproduzione
        if (_onlyRotateWhileTalking && _audioSource != null && !_audioSource.isPlaying) return;

        // Calcola la direzione verso il player
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        
        // Importante: azzeriamo la Y per evitare che l'NPC si ribalti in avanti o indietro
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Slerp permette una rotazione fluida invece di uno scatto secco
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }

    private void ExecuteCall()
    {
        _playerInRange = true;
        _lastCallTime = Time.time;

        if (_animator != null) _animator.SetTrigger(callTriggerName);

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
        
        Debug.Log($"{gameObject.name} sta chiamando e ruotando verso il Player!");
    }
}