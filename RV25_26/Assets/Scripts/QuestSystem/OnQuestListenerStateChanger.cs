using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestState
{
    public int questIndex;
    public string state;
}

[RequireComponent(typeof(IStateChangeable))]
public class OnQuestListenerStateChanger: OnQuestListener
{
    [SerializeField] private List<QuestState> _questStates;
    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        string state = null;
        foreach(QuestState qc in _questStates)
        {
            if(qc.questIndex == questIndex)
            {
                state = qc.state;
                break;
            }
        }

        if(state == null) return;

        IStateChangeable stateChangeable = GetComponent<IStateChangeable>();
        stateChangeable.ChangeState(state);
    }
}