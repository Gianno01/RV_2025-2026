using UnityEngine;

/// <summary>
/// MenuController reagisce all'evento _onStart e carica la main scene scelta. L'evento _onStart è invocato dal MenuUI.
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField] private AppEventData _onStart;
    [SerializeField] private string _mainScene;
    [SerializeField] private AppEventData _onStartScene;
    [SerializeField] private AppEventData _onHomeExit;
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private BlackScreenFader _blackScreenFader;

    void Awake()
    {
        _onStart.OnEvent += HandleOnStart;
    }

    void OnDisable()
    {
        _onStart.OnEvent -= HandleOnStart;
    }

    private void HandleOnStart()
    {
        _onStartScene.RaiseWithParam(_mainScene);
    }

    public void ExitHome()
    {
        _blackScreenFader.InitToMinEffect();
        _blackScreenFader.FadeIn();
        this.enabled = false;
    }

    public void EnterHome()
    {
        _onFadeInEnd.OnEvent += HandleFadeInEnd;
        _blackScreenFader.InitToMaxEffect();
        _blackScreenFader.FadeOut();
        this.enabled = true;
    }

    private void HandleFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEnd;
        _onHomeExit.Raise();
    }
}