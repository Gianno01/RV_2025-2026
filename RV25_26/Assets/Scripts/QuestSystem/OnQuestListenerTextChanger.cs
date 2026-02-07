using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestText
{
    public int questIndex;
    public SubtitleData subtitleData;
}

[RequireComponent(typeof(ITextChangeable))]
public class OnQuestListenerTextChanger : OnQuestListener
{
    [SerializeField] private List<QuestText> _questTexts;
    [SerializeField] private AppEventData _onTextChange;
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        SubtitleData subtitleData = null;
        foreach(QuestText qt in _questTexts)
        {
            if(qt.questIndex == questIndex)
            {
                subtitleData = qt.subtitleData;
                break;
            }
        }

        if(subtitleData == null) return;

        ITextChangeable textChangeable = GetComponent<ITextChangeable>();
        textChangeable.ChangeText(subtitleData);
        if(_onTextChange != null) _onTextChange.Raise();
    }
}