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
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        AudioClip clip = null;
        foreach(QuestClip qc in _questClips)
        {
            if(qc.questIndex == questIndex)
            {
                clip = qc.clip;
                break;
            }
        }

        if(clip == null) return;

        IClipChangeable clipChangeable = GetComponent<IClipChangeable>();
        clipChangeable.ChangeClip(clip);
    }
}