using UnityEngine;
using UnityEngine.Playables;

public class TimelineSpot : MonoBehaviour, ITimelineChangeable
{
    [SerializeField] private PlayableAsset _currentTimeline;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private AppEventData _onTimelineStart;
    [SerializeField] private AppEventData _onTimelineEnd;

    [Header("Se attivo, ferma la timeline in esecuzione e avvia la nuova, altrimenti il viceversa")]
    [SerializeField] private bool _stopCurrentClip;
    public void ChangeTimeline(PlayableAsset timeline)
    {
        _currentTimeline = timeline;
    }

    public void PlayTimeline()
    {
        if(_playableDirector.state == PlayState.Playing && !_stopCurrentClip) return;
        if(_currentTimeline != null) _playableDirector.Play(_currentTimeline);
    }

    public void HandleSignalOnTimelineStart()
    {
        if(_onTimelineStart != null) _onTimelineStart.Raise();
    }

    public void HandleSignalOnTimelineEnd()
    {
        if(_onTimelineEnd != null) _onTimelineEnd.Raise();
    }
}