using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private MenuUI _menuUI;
    [SerializeField] private AppEventData _onStart;
    [SerializeField] private Scene _mainScene;

    void Start()
    {
        _onStart.OnEvent += HandleOnStart;
    }

    private void HandleOnStart()
    {
        SceneManager.LoadScene(_mainScene.name);
    }
}