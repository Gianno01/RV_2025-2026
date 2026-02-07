using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpot : MonoBehaviour, IClipChangeable
{
    [SerializeField] private AppEventData _onAudio;

    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        AudioParam audioParam;
        audioParam.audioClip = _clip;
        audioParam.audioSource = _audioSource;

        _onAudio.RaiseWithParam(audioParam);
    }

    public void ChangeClip(AudioClip clip)
    {
        if (clip != null)
        {
            _clip = clip;
        }
    }
}