using UnityEngine;

public class AudioSpot : MonoBehaviour
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
}