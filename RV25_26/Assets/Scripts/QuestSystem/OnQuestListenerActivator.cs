using System.Linq;
using UnityEngine;
public class OnQuestListenerActivator : OnQuestListener
{
    [SerializeField] private int[] _startQuests;
    [SerializeField] private int[] _endQuests;
    [SerializeField] private bool _effectOnOneComponent;
    [SerializeField] private string _componentName;
    [SerializeField] private AppEventData _onActivate;
    [SerializeField] private AppEventData _onDeactivate;

    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;
        Debug.Log(questIndex);
        if (_startQuests.Contains(questIndex))
        {
            SetActivation(true);
            if(_onActivate != null) _onActivate.Raise();
        }
        else if (_endQuests.Contains(questIndex))
        {
            if(_onActivate != null) _onDeactivate.Raise();
            SetActivation(false);
        }
    }

    private void SetActivation(bool active)
    {
        if (_effectOnOneComponent)
        {
            MonoBehaviour component = (MonoBehaviour) gameObject.GetComponent(_componentName);
            component.enabled = active;
        }
        else
        {
            gameObject.SetActive(active);
        }
    }
}