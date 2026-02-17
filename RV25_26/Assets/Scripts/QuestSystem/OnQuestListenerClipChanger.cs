using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestClip
{
    public int questIndex;
    public AudioClip clip;
}

[RequireComponent(typeof(IClipChangeable))]
public class OnQuestListenerClipChanger : OnQuestListener
{
    [SerializeField] private List<QuestClip> _questClips;
    [SerializeField] private AppEventData _onClipChange;
    private bool _questFound;
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        AudioClip clip = null;
        _questFound = false;
        foreach(QuestClip qc in _questClips)
        {
            if(qc.questIndex == questIndex)
            {
                clip = qc.clip;
                _questFound = true;
                break;
            }
        }

        if(!_questFound || clip == null) return;

        IClipChangeable clipChangeable = GetComponent<IClipChangeable>();
        clipChangeable.ChangeClip(clip);
        if(_onClipChange != null) _onClipChange.Raise();
    }
}