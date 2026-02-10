using UnityEngine;

/// <summary>
/// GameplayController dovrebbe occuparsi di attivare e disattivare altri controller dello stato di gameplay
/// Ancora non è ben chiaro, potrebbe servire in futuro
/// </summary>
public class GameplayController : MonoBehaviour
{
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private AppEventData _onGameplayExit;
    [SerializeField] private AppEventData _onMasterExit;
    private QuestController _questController;
    private VFXController _VFXController;
    private GameObject _player;

    public void Init()
    {
        _questController = GameObject.FindAnyObjectByType<QuestController>();
        _player = GameObject.FindAnyObjectByType<MotionController>().gameObject;
        _VFXController = GameObject.FindAnyObjectByType<VFXController>();
        this.enabled = false;
    }

    public void ExitGameplay(AppState nextAppState)
    {
        if(nextAppState == AppState.Cutscene) _onFadeInEnd.OnEvent += HandleFadeInEnd;
        else if(nextAppState == AppState.Gate)
        {
            _player.GetComponent<MotionController>().enabled = false;
            _player.GetComponent<InteractionController>().enabled = false;
            CompleteToExitGameplay(nextAppState);
        }else if (nextAppState == AppState.Home)
        {
            _onFadeInEnd.OnEvent += HandleFadeInEndToHome;
        }else if (nextAppState == AppState.End)
        {
            _onFadeInEnd.OnEvent += HandleFadeInEndToEnd;
        }

        _VFXController.PlayChangeAppStateFadeIn();
    }

    public void EnterGameplay(AppState appState)
    {
        this.enabled = true;

        _player.SetActive(true);

        if(appState == AppState.Gate)
        {
            _player.GetComponent<MotionController>().enabled = true;
            _player.GetComponent<InteractionController>().enabled = true;
        }

        if(appState == AppState.Home)
        {
            _questController.Init();
            return;
        }
        
        _VFXController.PlayChangeAppStateFadeOut();
    }

    private void HandleFadeInEndToHome()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEndToHome;

        FromSceneToScene fromSceneToScene;
        fromSceneToScene.from = "MasterScene";
        fromSceneToScene.to = "HomeScene";
        _onMasterExit.RaiseWithParam(fromSceneToScene);
    }

    private void HandleFadeInEndToEnd()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEndToEnd;

        FromSceneToScene fromSceneToScene;
        fromSceneToScene.from = "MasterScene";
        fromSceneToScene.to = "EndCreditsScene";
        _onMasterExit.RaiseWithParam(fromSceneToScene);
    }

    private void HandleFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEnd;
        _player.SetActive(false);
        CompleteToExitGameplay(AppState.Cutscene);
    }

    private void CompleteToExitGameplay(AppState nextAppState)
    {
        _onGameplayExit.RaiseWithParam(nextAppState);
        this.enabled = false;
    }
}