using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MenuController reagisce all'evento _onStart e carica la main scene scelta. L'evento _onStart è invocato dal MenuUI.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField] private AppEventData _onStart;
    [SerializeField] private string _mainScene;

    void Start()
    {
        _onStart.OnEvent += HandleOnStart;
    }

    private void HandleOnStart()
    {
        SceneManager.LoadScene(_mainScene);
    }
}