using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class TalkingCharacter : MonoBehaviour, IsInteractable 
{
    [SerializeField] private AudioClip _dialogue;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AppEventData _onDialogue;
    public void Interact() {
        DialogueParam dialogueParam;
        dialogueParam.audioSource = _audioSource;
        dialogueParam.audioClip = _dialogue;
        
        _onDialogue.RaiseWithParam(dialogueParam);
    }
    public string GetDescription() { return "Oggetto di prova"; }
}