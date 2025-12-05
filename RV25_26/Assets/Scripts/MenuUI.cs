using UnityEngine;
using UnityEngine.UI;

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
        _onStart.Raise();
    }
}