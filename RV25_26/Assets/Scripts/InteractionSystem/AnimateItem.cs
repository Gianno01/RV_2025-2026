using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class AnimateItem : MonoBehaviour, IsInteractable 
{
    [SerializeField] private PlayableAsset[] timelines;
    private bool _toggleState = false;
    private PlayableDirector _director;
private Outline _outline;
void Awake() 
{
    // Recupera il componente Outline che deve essere presente sull'oggetto
    _outline = GetComponent<Outline>();
    if (_outline != null) _outline.enabled = false; // Partiamo con l'effetto spento
}
    void Start() 
    {
        _director = GetComponent<PlayableDirector>();
    }
    public void Interact() {
        _toggleState = !_toggleState;
        _director.Play(timelines[_toggleState ? 1 : 0]);
    }
    public string GetDescription() { return "Oggetto di prova"; }
public void OnFocus() 
{
    if (_outline != null) _outline.enabled = true;
}

public void OnLostFocus() 
{
    if (_outline != null) _outline.enabled = false;
}

}

