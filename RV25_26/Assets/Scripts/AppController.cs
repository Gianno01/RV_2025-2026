using UnityEngine;
using UnityEngine.SceneManagement;

public enum AppState{Gameplay, Cutscene, Home}

/// <summary>
/// AppController non si distrugge al caricamento delle scene. Va istanziato nella prima scena.
/// Al caricamento della scena 0 (home) della lista, cambia lo stato di gioco in Home e attiva l'_homeController della scena.
/// Al caricamento della scena 1 (main) della lista, cambia lo stato di gioco in Gameplay, attiva il _gameplayController
/// e disattiva il _cutsceneController, entrambi presenti nella scena. All'invocazione dell'evento _onCutsceneStart
/// cambia lo stato in Cutscene, attiva il _cutsceneController, disattiva il _gameplayController e avvia la timeline.
/// All'invocazione dell'evento _onGameplayResume cambia lo stato in Gameplay, attiva il _gameplayController, disattiva il _cutsceneController.
/// </summary>
public class AppController : MonoBehaviour
{
    [HideInInspector] public static AppController Instance { get; private set; }
    [SerializeField] private string[] _scenes;
    [SerializeField] private string[] _additiveScenes;
    [SerializeField] private AppEventData _onCutsceneStart;
    [SerializeField] private AppEventData _onGameplayResume;
    [SerializeField] private AppEventData _onGameplayExit;
    [SerializeField] private AppEventData _onCutsceneExit;
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
        //per test, da commentare
        HandleOnSceneLoaded(SceneManager.GetActiveScene(),LoadSceneMode.Single);
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == _scenes[0]){
            _menuController = GameObject.FindAnyObjectByType<MenuController>();
            _menuController.enabled = false;
            ToHomeState();
        }else if(scene.name == _scenes[1]){
            LoadAdditiveScenes();

            _gameplayController = GameObject.FindAnyObjectByType<GameplayController>();
            _cutsceneController = GameObject.FindAnyObjectByType<CutsceneController>();
            _gameplayController.Init();
            _cutsceneController.Init();
            ToGameplayState();
        }
    }

    private void LoadAdditiveScenes()
    {
        foreach(string s in _additiveScenes)
        {
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
        }
    }

    private void HandleCutsceneStart()
    {
        _onGameplayExit.OnEvent += HandleOnGameplayExit;
        _gameplayController.ExitGameplay();
    }

    private void HandleOnGameplayResume()
    {
        _onCutsceneExit.OnEvent += HandleOnCutsceneExit;
        _cutsceneController.ExitCutscene();
    }

    private void HandleOnGameplayExit()
    {
        _onGameplayExit.OnEvent -= HandleOnGameplayExit;

        ToCutsceneState();
        _cutsceneController.PlayCurrentTimeline();
    }

    private void HandleOnCutsceneExit()
    {
        _onCutsceneExit.OnEvent -= HandleOnCutsceneExit;

        ToGameplayState();
    }

   // i tre metodi di seguito sono callback agli eventi che producono un cambio di stato
    private void ToGameplayState()
    {
        _gameplayController.EnterGameplay(_currentAppState);
        _currentAppState = AppState.Gameplay;

        if(_onGameplayResume.OnEvent != null) _onGameplayResume.OnEvent -= HandleOnGameplayResume;
        _onCutsceneStart.OnEvent += HandleCutsceneStart;
    }
    private void ToCutsceneState()
    {
        _currentAppState = AppState.Cutscene;
        _cutsceneController.EnterCutscene();

        if(_onCutsceneStart.OnEvent != null) _onCutsceneStart.OnEvent -= HandleCutsceneStart;
        _onGameplayResume.OnEvent += HandleOnGameplayResume;
    }
    private void ToHomeState()
    {
        _currentAppState = AppState.Home;
        _menuController.enabled = true;
    }
}