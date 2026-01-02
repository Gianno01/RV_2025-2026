using UnityEngine;

public struct SpatialParam{
    public AudioSource audioSource;
    public AudioClip audioClip; 
}

/// <summary>
/// AudioController si occupa di riprodurre le clip audio. 
/// Un audio non spazializzato viene riprodotto come reazione all'evento _onAudio, un audio spazializzato come reazione all'evento _onSpatialAudio.
/// Nel caso di audio spazializzato, occorre passare con l'evento la sorgente audio e la clip. 
/// Per quello non spazializzato solo la clip che viene riprodotta dalla sorgente presente sul Gameobject del controller.
/// Per distinguere tra dialoghi, monologhi, sfx e rumori ambientali creare un audio controller per ciascuno
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AppEventData _onSpatialAudio;
    [SerializeField] private AppEventData _onAudio;
    private AudioSource _noSpatialAudioSource;

    void Awake()
    {
        if (_onSpatialAudio)
        {
            _onSpatialAudio.OnParamEvent += HandleOnSpatialAudio;
        }

        if (_onAudio)
        {
            _onAudio.OnParamEvent += HandleOnAudio;
            _noSpatialAudioSource = gameObject.GetComponent<AudioSource>();
            _noSpatialAudioSource.spatialBlend = 0;
        }
    }

    void OnDisable()
    {
        if (_onSpatialAudio)
        {
            _onSpatialAudio.OnParamEvent -= HandleOnSpatialAudio;
        }

        if (_onAudio)
        {
            _onAudio.OnParamEvent -= HandleOnAudio;
        }
    }

    private void HandleOnSpatialAudio(object param)
    {
        SpatialParam spatialParam = (SpatialParam) param;
        spatialParam.audioSource.PlayOneShot(spatialParam.audioClip);
    }

    private void HandleOnAudio(object param)
    {
        AudioClip clip = (AudioClip) param;
        _noSpatialAudioSource.PlayOneShot(clip);
    }
}