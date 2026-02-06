using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MenuUI invoca l'evento _onStart se premuto il bottone _startButton.
/// </summary>
public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private AppEventData _onStart;

    void Start()
    {
        _startButton.onClick.AddListener(HandleStartButtonClick);
    }

    void OnDisable()
    {
        _startButton.onClick.RemoveListener(HandleStartButtonClick);
    }

    private void HandleStartButtonClick()
    {
        Debug.Log("STEP -4");
        _onStart.Raise();
    }
}