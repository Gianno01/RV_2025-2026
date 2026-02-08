using UnityEngine;

public class QuestAdder : MonoBehaviour
{
    [SerializeField] private AppEventData _onQuestStart;

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _onQuestStart.Raise();
        }
    }
}