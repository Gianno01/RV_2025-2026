using UnityEngine;

public class NpcProximityCaller : MonoBehaviour
{
    [Header("Risorse Dirette")]
    [SerializeField] private AudioClip _voiceClip; // La risorsa audio diretta
    [SerializeField] private SubtitleData _subtitle; // Il file dei sottotitoli
    [SerializeField] private AppEventData _onSubtitleShow; // L'evento che invia il testo alla UI

    [Header("Riferimenti Fisici")]
    [SerializeField] private AudioSource _audioSource; //emette il suono
    private Animator _animator;
    private NpcBrain _brain;

    [Header("Parametri Sensore")]
    [SerializeField] private float detectionRange = 7.0f; 
    [SerializeField] private float cooldownTime = 10.0f; 
    
    [Header("Animazione")]
    [SerializeField] private string callTriggerName = "call"; 

    private Transform _playerTransform;
    private float _lastCallTime = -100f;
    private bool _playerInRange = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _brain = GetComponent<NpcBrain>();
        
        // Se non assegnato, prova a prenderlo dall'oggetto
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

        if (distance <= detectionRange && !_playerInRange && Time.time >= _lastCallTime + cooldownTime)
        {
            if (_brain != null && _brain.currentState == NpcState.Talk) return;
            ExecuteCall();
        }
        else if (distance > detectionRange)
        {
            _playerInRange = false;
        }
    }

    private void ExecuteCall()
    {
        _playerInRange = true;
        _lastCallTime = Time.time;

        // 1. Animazione
        if (_animator != null) _animator.SetTrigger(callTriggerName);

        // 2. Audio Diretto
        if (_audioSource != null && _voiceClip != null)
        {
            _audioSource.clip = _voiceClip;
            _audioSource.Play();
        }

        // 3. Sottotitoli Diretti

        if (_onSubtitleShow != null && _subtitle != null)
        {
            SubtitleDataTimeReference st;
            st.subtitleData = _subtitle;
            st.audioSource = _audioSource;
            st.playableDirector = null;

            _onSubtitleShow.RaiseWithParam(st); // Invia il testo alla UI
        }
        
        Debug.Log($"{gameObject.name} sta chiamando con risorse dirette!");
    }
}