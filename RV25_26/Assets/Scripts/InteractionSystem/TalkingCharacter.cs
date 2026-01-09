using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))] // Aggiunto per gestire le animazioni
public class TalkingCharacter : MonoBehaviour, IsInteractable // Implementa l'interfaccia
{
    [Header("Configurazione Audio")]
    [SerializeField] private AudioClip _dialogueClip;
    [SerializeField] private AppEventData _onSpatialAudioEvent;

    [Header("Configurazione Animazione")]
    [SerializeField] private string _animationTriggerName = "Talk"; // Nome del trigger nell'Animator

    private AudioSource _myAudioSource;
    private Animator _myAnimator; // Riferimento all'Animator

    void Awake() // Usiamo Awake per essere sicuri che l'audio sia pronto
    {
        _myAudioSource = GetComponent<AudioSource>();
        _myAnimator = GetComponent<Animator>(); // Inizializziamo l'Animator

        _myAudioSource.spatialBlend = 1.0f; // Forza l'audio 3D
    }

    // Implementazione del metodo richiesto dall'interfaccia IsInteractable
    public string GetDescription()
    {
        return "Parla con il personaggio";
    }

    // Questo metodo viene chiamato dal InteractionController tramite l'interfaccia
    public void Interact()
    {
        // Avvia l'animazione se l'Animator è presente
        if (_myAnimator != null)
        {
            _myAnimator.SetTrigger(_animationTriggerName);
        }

        if (_dialogueClip == null || _onSpatialAudioEvent == null) return;

        SpatialParam dialogueParam;
        dialogueParam.audioSource = _myAudioSource;
        dialogueParam.audioClip = _dialogueClip;

        _onSpatialAudioEvent.RaiseWithParam(dialogueParam);
    }
}