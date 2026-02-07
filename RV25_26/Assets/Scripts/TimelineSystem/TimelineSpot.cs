using UnityEngine;
using UnityEngine.Playables;

public class TimelineSpot : MonoBehaviour, ITimelineChangeable
{
    [SerializeField] private PlayableAsset _currentTimeline;
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private AppEventData _onTimelineStart;
    [SerializeField] private AppEventData _onTimelineEnd;
    public void ChangeTimeline(PlayableAsset timeline)
    {
        _currentTimeline = timeline;
    }

    public void PlayTimeline()
    {
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