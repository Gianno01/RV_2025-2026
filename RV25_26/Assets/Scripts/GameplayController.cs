using UnityEngine;

/// <summary>
/// GameplayController dovrebbe occuparsi di attivare e disattivare altri controller dello stato di gameplay
/// Ancora non è ben chiaro, potrebbe servire in futuro
/// </summary>
public class GameplayController : MonoBehaviour
{
    private QuestController _questController;

    void Start()
    {
        _questController = GameObject.FindAnyObjectByType<QuestController>();
    }
}