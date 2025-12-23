using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TalkingCharacter : MonoBehaviour 
{
    [Header("Configurazione Audio")]
    [SerializeField] private AudioClip _dialogueClip;
    [SerializeField] private AppEventData _onSpatialAudioEvent;

    private AudioSource _myAudioSource;

    void Awake() // Usiamo Awake per essere sicuri che l'audio sia pronto
    {
        _myAudioSource = GetComponent<AudioSource>();
        _myAudioSource.spatialBlend = 1.0f; // Forza l'audio 3D
    }

    // Questo metodo viene chiamato dal NpcBrain
    public void Interact() 
    {
        if (_dialogueClip == null || _onSpatialAudioEvent == null) return;

        SpatialParam dialogueParam;
        dialogueParam.audioSource = _myAudioSource;
        dialogueParam.audioClip = _dialogueClip;

        _onSpatialAudioEvent.RaiseWithParam(dialogueParam);
    }
}