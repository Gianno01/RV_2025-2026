using UnityEngine;

public class AudioSpot : MonoBehaviour
{
    [SerializeField] private bool _isSpatialAudio;
    [SerializeField] private AppEventData _onSpatialAudio;
    [SerializeField] private AppEventData _onAudio;

    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1.0f;
    }

    public void PlayAudio()
    {
        if (_isSpatialAudio)
        {
            SpatialParam spatialParam;
            spatialParam.audioClip = _clip;
            spatialParam.audioSource = _audioSource;

            _onSpatialAudio.RaiseWithParam(spatialParam);
        }
        else
        {
            _onAudio.RaiseWithParam(_clip);
        }
    }
}