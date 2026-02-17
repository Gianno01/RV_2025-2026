using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public struct QuestTimeline
{
    public int questIndex;
    public PlayableAsset timeline;
}

[RequireComponent(typeof(ITimelineChangeable))]
public class OnQuestListenerTimelineChanger : OnQuestListener
{
    [SerializeField] private List<QuestTimeline> _questTimelines;
    [SerializeField] private AppEventData _onTimelineChange;
    private bool _questFound;
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        PlayableAsset timeline = null;
        _questFound = false;
        foreach(QuestTimeline qt in _questTimelines)
        {
            if(qt.questIndex == questIndex)
            {
                timeline = qt.timeline;
                _questFound = true;
                break;
            }
        }

        if(!_questFound || timeline == null) return;

        ITimelineChangeable timelineChangeable = GetComponent<ITimelineChangeable>();
        timelineChangeable.ChangeTimeline(timeline);
        if(_onTimelineChange != null) _onTimelineChange.Raise();
    }
}