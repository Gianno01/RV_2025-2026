using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpot : MonoBehaviour, IClipChangeable
{
    [SerializeField] private AppEventData _onAudioStart;
    [SerializeField] private AppEventData _onAudioEnd;

    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if(_clip == null) return;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_clip);

        if(_onAudioStart != null) _onAudioStart.Raise();
        DOVirtual.DelayedCall(_clip.length, () =>
        {
            if(_onAudioEnd != null) _onAudioEnd.Raise();
        });
    }

    public void ChangeClip(AudioClip clip)
    {
        if (clip != null)
        {
            _clip = clip;
        }
    }
}