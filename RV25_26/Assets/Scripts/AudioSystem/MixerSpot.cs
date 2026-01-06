using UnityEngine;
using UnityEngine.Audio;

public enum SoundType{Ambience, SFX, Voice}

public class MixerSpot : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot snapshotToActivate;
    [SerializeField] private AppEventData OnSnapshotToChange;

    public void ChangeAudioSnapshot()
    {
        OnSnapshotToChange.RaiseWithParam(snapshotToActivate);
    }
}