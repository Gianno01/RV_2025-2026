using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))] 
public class TalkingCharacter : MonoBehaviour, IsInteractable, IClipChangeable // Aggiunta l'interfaccia IClipChangeable
{
    [Header("Configurazione Audio")]
    [SerializeField] private AudioClip _dialogueClip;
    [SerializeField] private AppEventData _onSpatialAudioEvent;

    [Header("Configurazione Animazione")]
    [SerializeField] private string _animationTriggerName = "Talk"; 

    [SerializeField] private AppEventData _onDialogueStart;

    private AudioSource _myAudioSource;
    private Animator _myAnimator; 
private Outline _outline;
    void Awake() 
    {
        _myAudioSource = GetComponent<AudioSource>();
        _myAnimator = GetComponent<Animator>(); 

        _myAudioSource.spatialBlend = 1.0f; 
        _outline = GetComponent<Outline>();
    if (_outline != null) _outline.enabled = false; // Partiamo con l'effetto spento
    }

    // --- IMPLEMENTAZIONE IClipChangeable ---

    public void ChangeClip(AudioClip clip) // Implementazione del metodo dell'interfaccia
    {
        if (clip != null)
        {
            _dialogueClip = clip; // Aggiorna la clip con quella nuova ricevuta
        }
    }

    public string GetDescription()
    {
        return "Parla con il personaggio";
    }

    public void Interact()
    {
        if (_myAnimator != null)
        {
            _myAnimator.SetTrigger(_animationTriggerName);
        }

        if (_dialogueClip == null || _onSpatialAudioEvent == null) return;

        SpatialParam dialogueParam;
        dialogueParam.audioSource = _myAudioSource;
        dialogueParam.audioClip = _dialogueClip;

        _onSpatialAudioEvent.RaiseWithParam(dialogueParam);
        if(_onDialogueStart != null) _onDialogueStart.Raise();
    }

    public void OnFocus() 
{
    if (_outline != null) _outline.enabled = true;
}

public void OnLostFocus() 
{
    if (_outline != null) _outline.enabled = false;
}
}