using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AppState{Gameplay, Cutscene, Home, Gate}

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
    [SerializeField] private AppEventData _onGateStart;
    [SerializeField] private AppEventData _onGateExit;
    [SerializeField] private AppEventData _onGameplayExit;
    [SerializeField] private AppEventData _onCutsceneExit;
    private GameplayController _gameplayController;
    private CutsceneController _cutsceneController;
    private TransitionController _transitionController;
    private MenuController _menuController;
    private AppState _currentAppState;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _currentAppState = AppState.Home;
        }else{
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
        //per test, da commentare
        //HandleOnSceneLoaded(SceneManager.GetActiveScene(),LoadSceneMode.Single);
    }

    private void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == _scenes[0]){
            _menuController = GameObject.FindAnyObjectByType<MenuController>();
            _menuController.enabled = false;
            ToHomeState();
        }else if(scene.name == _scenes[1]){
            StartCoroutine(LoadAdditiveScenes());
        }
    }

    private IEnumerator LoadAdditiveScenes() { 
        List<AsyncOperation> ops = new List<AsyncOperation>();
        foreach (string s in _additiveScenes) {
            AsyncOperation op = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
            op.allowSceneActivation = true; 
            ops.Add(op); 
        } 
        foreach (var op in ops) { 
            while (!op.isDone) 
                yield return null; 
        } 

        _gameplayController = GameObject.FindAnyObjectByType<GameplayController>();
        _cutsceneController = GameObject.FindAnyObjectByType<CutsceneController>();
        _transitionController = GameObject.FindAnyObjectByType<TransitionController>();

        _gameplayController.Init();
        _cutsceneController.Init();
        ToGameplayState();
    }

    private void HandleGateStart()
    {
        if(_currentAppState != AppState.Gameplay) return;

        _onGameplayExit.OnParamEvent += HandleOnGameplayExit;
        _gameplayController.ExitGameplay(AppState.Gate);
    }

    private void HandleCutsceneStart()
    {
        _onGameplayExit.OnParamEvent += HandleOnGameplayExit;
        _gameplayController.ExitGameplay(AppState.Cutscene);
    }

    private void HandleOnGameplayResume()
    {
        if(_currentAppState == AppState.Cutscene)
        {
            _onCutsceneExit.OnEvent += HandleOnCutsceneExit;
            _cutsceneController.ExitCutscene();
        }else if(_currentAppState == AppState.Gate)
        {
            _transitionController.ExitGate();
        }
    }

    private void HandleOnGameplayExit(object param)
    {
        if(_currentAppState != AppState.Gameplay) return;
        AppState nextState = (AppState) param;
        _onGameplayExit.OnParamEvent -= HandleOnGameplayExit;

        if(nextState == AppState.Cutscene)
        {
            ToCutsceneState();
            _cutsceneController.PlayCurrentTimeline();
        }
        else if(nextState == AppState.Gate) ToGateState();
    }

    private void HandleOnCutsceneExit()
    {
        _onCutsceneExit.OnEvent -= HandleOnCutsceneExit;

        ToGameplayState();
    }

    private void HandleOnGateExit()
    {
        if(_currentAppState != AppState.Gate) return;
        ToGameplayState();
    }

   // i tre metodi di seguito sono callback agli eventi che producono un cambio di stato
    private void ToGameplayState()
    {
        _gameplayController.EnterGameplay(_currentAppState);
        _currentAppState = AppState.Gameplay;

        if(_onGameplayResume.OnEvent != null) _onGameplayResume.OnEvent -= HandleOnGameplayResume;
        _onCutsceneStart.OnEvent += HandleCutsceneStart;
        _onGateStart.OnEvent += HandleGateStart;
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

    private void ToGateState()
    {
        _currentAppState = AppState.Gate;
        _transitionController.EnterGate();
        
        if(_onGateStart.OnEvent != null) _onGateStart.OnEvent -= HandleGateStart;
        _onGateExit.OnEvent += HandleOnGateExit;
        
        _onGameplayResume.OnEvent += HandleOnGameplayResume;
    }
}