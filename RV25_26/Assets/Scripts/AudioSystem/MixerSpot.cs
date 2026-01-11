using UnityEngine;
using UnityEngine.Audio;

public enum SoundType{Ambience, SFX, Voice}

public class MixerSpot : MonoBehaviour
{
    [SerializeField] protected AudioMixerSnapshot snapshotToActivate;
    [SerializeField] private AppEventData OnSnapshotToChange;

    public virtual void ChangeAudioSnapshot()
    {
        OnSnapshotToChange.RaiseWithParam(snapshotToActivate);
    }
}