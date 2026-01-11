using UnityEngine;
using UnityEngine.Audio;

public class MixerSpotToggle : MixerSpot
{
    [SerializeField] private AudioMixerSnapshot[] _snapshots;
    private bool _toggle = false;

    public override void ChangeAudioSnapshot()
    {
        snapshotToActivate = _snapshots[_toggle ? 1 : 0];
        _toggle = !_toggle;
        base.ChangeAudioSnapshot();
    }
}