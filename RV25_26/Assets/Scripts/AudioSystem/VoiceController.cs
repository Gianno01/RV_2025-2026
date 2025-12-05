using UnityEngine;

public struct DialogueParam{
    public AudioSource audioSource;
    public AudioClip audioClip;
}

/// <summary>
/// VoiceController si occupa di riprodurre le clip di dialoghi e monologhi. 
/// Un dialogo viene riprodotto come reazione all'evento _onDialogue, un monologo come reazione all'evento _onMonologue.
/// Nel caso del dialogo, occorre passare con l'evento la sorgente audio e la clip in quanto spazializzato. 
/// Per il monologo solo la clip che viene riprodotta dalla sorgente presente sul Gameobject del controller.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class VoiceController : MonoBehaviour
{
    [SerializeField] private AppEventData _onDialogue;
    [SerializeField] private AppEventData _onMonologue;
    private AudioSource _noSpatialAudioSource;

    void Start()
    {
        _onDialogue.OnParamEvent += HandleOnDialogue;
        _onMonologue.OnParamEvent += HandleOnMonologue;
        _noSpatialAudioSource = gameObject.GetComponent<AudioSource>();
        _noSpatialAudioSource.spatialBlend = 0;
    }

    void OnDisable()
    {
        _onDialogue.OnParamEvent -= HandleOnDialogue;
        _onMonologue.OnParamEvent -= HandleOnMonologue;
    }

    private void HandleOnDialogue(object param)
    {
        DialogueParam dialogueParam = (DialogueParam) param;
        dialogueParam.audioSource.PlayOneShot(dialogueParam.audioClip);
    }

    private void HandleOnMonologue(object param)
    {
        AudioClip clip = (AudioClip) param;
        Debug.Log("CLIP NAME: " + clip.name);
        _noSpatialAudioSource.PlayOneShot(clip);
    }
}