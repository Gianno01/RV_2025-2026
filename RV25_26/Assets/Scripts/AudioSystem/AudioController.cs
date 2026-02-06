using UnityEngine;

public struct AudioParam{
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
    [SerializeField] private AppEventData _onAudio;

    void Awake()
    {
        if (_onAudio)
        {
            _onAudio.OnParamEvent += HandleOnAudio;
        }
    }

    void OnDisable()
    {
        if (_onAudio)
        {
            _onAudio.OnParamEvent -= HandleOnAudio;
        }
    }

    private void HandleOnAudio(object param)
    {
        AudioParam audioParam = (AudioParam) param;
        if(audioParam.audioClip == null) return;

        audioParam.audioSource.Stop();
        audioParam.audioSource.PlayOneShot(audioParam.audioClip);
    }
}