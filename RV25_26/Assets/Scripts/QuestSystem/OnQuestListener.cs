using UnityEngine;
public abstract class OnQuestListener : MonoBehaviour
{
    [SerializeField] private AppEventData _onCurrentQuestChange;

    void Awake()
    {
        _onCurrentQuestChange.OnParamEvent += HandleOnQuestChange;
    }

    void OnDisable()
    {
        _onCurrentQuestChange.OnParamEvent -= HandleOnQuestChange;
    }

    protected virtual void HandleOnQuestChange(object param)
    {
        
    }
}