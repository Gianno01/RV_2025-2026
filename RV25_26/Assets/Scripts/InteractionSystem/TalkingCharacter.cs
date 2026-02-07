using UnityEngine;

[RequireComponent(typeof(TextSpot))] 
[RequireComponent(typeof(TimelineSpot))] 
public class TalkingCharacter : MonoBehaviour, IsInteractable
{
    [Header("Configurazione Sottotitoli")]
    [SerializeField] private TextSpot _textSpot;

    [Header("Configurazione Animazione")]
    [SerializeField] private TimelineSpot _timelineSpot;

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
        _timelineSpot.PlayTimeline();
        _textSpot.ShowText();
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