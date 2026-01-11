using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class AnimateItem : MonoBehaviour, IsInteractable 
{
    [SerializeField] private PlayableAsset[] timelines;
    private bool _toggleState = false;
    private PlayableDirector _director;

    void Start() 
    {
        _director = GetComponent<PlayableDirector>();
    }
    public void Interact() {
        //Debug.Log("Ho interagito con l'oggetto!");
        // Qui metterai il codice per raccogliere/aprire
        //Destroy(gameObject); // Esempio: distrugge l'oggetto
        _toggleState = !_toggleState;
        _director.Play(timelines[_toggleState ? 1 : 0]);
    }
    public string GetDescription() { return "Oggetto di prova"; }
}