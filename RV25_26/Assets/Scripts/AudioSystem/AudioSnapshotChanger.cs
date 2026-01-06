using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotChanger: MonoBehaviour
{
    [SerializeField] private float snapshotTransitionTime;
    [SerializeField] private AppEventData OnSnapshotToChange;

    void Awake()
    {
        OnSnapshotToChange.OnParamEvent += ChangeAudioSnapshot;
    }
    public void ChangeAudioSnapshot(object param)
    {
        AudioMixerSnapshot snapshot = (AudioMixerSnapshot) param;
        snapshot.TransitionTo(snapshotTransitionTime);
    }
}