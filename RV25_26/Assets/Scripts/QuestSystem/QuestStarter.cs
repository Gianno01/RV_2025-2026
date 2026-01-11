using UnityEngine;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private AppEventData _onEvent;
    [SerializeField] private AppEventData _onQuestStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        _onEvent.OnEvent += HandleOnEvent;
    }

    void OnDisable()
    {
        _onEvent.OnEvent -= HandleOnEvent;
    }

    private void HandleOnEvent()
    {
        _onQuestStart.Raise();
    }
}
