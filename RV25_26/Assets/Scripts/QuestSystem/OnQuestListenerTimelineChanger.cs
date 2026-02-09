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
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        PlayableAsset timeline = null;
        foreach(QuestTimeline qt in _questTimelines)
        {
            if(qt.questIndex == questIndex)
            {
                timeline = qt.timeline;
                break;
            }
        }

        if(timeline == null) return;

        ITimelineChangeable timelineChangeable = GetComponent<ITimelineChangeable>();
        timelineChangeable.ChangeTimeline(timeline);
        if(_onTimelineChange != null) _onTimelineChange.Raise();
    }
}