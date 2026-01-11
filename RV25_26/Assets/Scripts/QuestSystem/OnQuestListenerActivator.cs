using System.Linq;
using UnityEngine;
public class OnQuestListenerActivator : OnQuestListener
{
    [SerializeField] private int[] _startQuests;
    [SerializeField] private int[] _endQuests;
    [SerializeField] private bool _effectOnOneComponent;
    [SerializeField] private string _componentName;

    protected override void HandleOnQuestChange(object param)
    {
        int questIndex = (int) param;

        if (_startQuests.Contains(questIndex))
        {
            SetActivation(true);
        }
        else if (_endQuests.Contains(questIndex))
        {
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