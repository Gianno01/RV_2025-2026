using UnityEngine;

/// <summary>
/// GameplayController dovrebbe occuparsi di attivare e disattivare altri controller dello stato di gameplay
/// Ancora non è ben chiaro, potrebbe servire in futuro
/// </summary>
public class GameplayController : MonoBehaviour
{
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private AppEventData _onGameplayExit;
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

    public void ExitGameplay()
    {
        _onFadeInEnd.OnEvent += HandleFadeInEnd;
        _VFXController.PlayChangeAppStateFadeIn();
    }

    public void EnterGameplay(AppState appState)
    {
        this.enabled = true;
        _player.SetActive(true);

        if(appState == AppState.Home)
        {
            _questController.Init();
            return;
        }   

        _VFXController.PlayChangeAppStateFadeOut();
    }

    private void HandleFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEnd;
        _player.SetActive(false);
        _onGameplayExit.Raise();
        this.enabled = false;
    }
}