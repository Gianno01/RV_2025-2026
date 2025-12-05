using UnityEngine;

public class GameplayController : MonoBehaviour
{
    private QuestController _questController;

    void Start()
    {
        _questController = GameObject.FindAnyObjectByType<QuestController>();
    }
}