using UnityEngine;

[RequireComponent(typeof(AudioSpot))] 
[RequireComponent(typeof(TextSpot))] 
[RequireComponent(typeof(Animator))] 
public class TalkingCharacter : MonoBehaviour, IsInteractable
{
    [Header("Configurazione Audio")]
    [SerializeField] private AudioSpot _audioSpot;
    
    [Header("Configurazione Sottotitoli")]
    [SerializeField] private TextSpot _textSpot;

    [Header("Configurazione Animazione")]
    [SerializeField] private string _animationTriggerName = "Talk";
    [SerializeField] private Animator _myAnimator;  

    [SerializeField] private AppEventData _onDialogueStart;

    [SerializeField] private Outline _outline;

    void Awake() 
    {
        if (_outline != null) _outline.enabled = false; // Partiamo con l'effetto spento
    }

    // --- IMPLEMENTAZIONE IClipChangeable ---

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

        _audioSpot.PlayAudio();
        _textSpot.ShowText();
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