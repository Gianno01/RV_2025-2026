using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpot : MonoBehaviour, IClipChangeable
{
    [SerializeField] private AppEventData _onAudioStart;
    [SerializeField] private AppEventData _onAudioEnd;

    [SerializeField] private AudioClip _clip;

    [Header("Se attivo, ferma la clip in esecuzione e avvia la nuova, altrimenti il viceversa")]
    [SerializeField] private bool _stopCurrentClip;
    [Header("Se attivo, permette la riproduzione di più clip in contemporanea. In genere non si vuole")]
    [SerializeField] private bool _playMoreThanOne;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if(_clip == null) return;
        
        if (_playMoreThanOne)
        {
            _audioSource.PlayOneShot(_clip);
        }
        else
        {
            if(!_stopCurrentClip && _audioSource.isPlaying) return;
            _audioSource.clip = _clip;
            _audioSource.Play();
        }

        if(_onAudioStart != null) _onAudioStart.Raise();
        DOVirtual.DelayedCall(_clip.length, () =>
        {
            if(_onAudioEnd != null) _onAudioEnd.Raise();
        });
    }

    public void StopAudio()
    {
        if(_audioSource.isPlaying)
            _audioSource.Stop();
    }

    public void ChangePitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }

    public void ChangeClip(AudioClip clip)
    {
        if (clip != null)
        {
            _clip = clip;
        }
    }
}