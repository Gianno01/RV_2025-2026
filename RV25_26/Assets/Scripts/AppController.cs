using UnityEngine;
using UnityEngine.SceneManagement;

public enum AppState{Gameplay, Cutscene, Home}

public class AppController : MonoBehaviour
{
    [HideInInspector] public static AppController Instance { get; private set; }
    [SerializeField] private Scene[] scenes;
    private GameplayController _gameplayController;
    private CutsceneController _cutsceneController;
    private MenuController _menuController;
    private AppState _currentAppState;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene == scenes[0]){
            _menuController = GameObject.FindAnyObjectByType<MenuController>();
            _menuController.enabled = false;
            ToHomeState();
        }else if(scene == scenes[1]){
            _gameplayController = GameObject.FindAnyObjectByType<GameplayController>();
            _cutsceneController = GameObject.FindAnyObjectByType<CutsceneController>();
            _gameplayController.enabled = false;
            _cutsceneController.enabled = false;
            ToGameplayState();
        }
    }

    // i tre metodi di seguito sono callback agli eventi che producono un cambio di stato
    private void ToGameplayState()
    {
        _currentAppState = AppState.Gameplay;
        _cutsceneController.enabled = false;
        _gameplayController.enabled = true;
    }

    private void ToCutscenState()
    {
        _currentAppState = AppState.Cutscene;
        _gameplayController.enabled = false;
        _cutsceneController.enabled = true;
    }
    private void ToHomeState()
    {
        _currentAppState = AppState.Home;
        _menuController.enabled = true;
    }
}