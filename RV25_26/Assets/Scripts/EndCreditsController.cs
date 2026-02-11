using UnityEngine;

/// <summary>
/// MenuController reagisce all'evento _onStart e carica la main scene scelta. L'evento _onStart è invocato dal MenuUI.
/// </summary>
public class EndCreditsController : MonoBehaviour
{
    [SerializeField] private string _homeScene;
    [SerializeField] private AppEventData _onHomeSceneRequest;
    [SerializeField] private AppEventData _onEndCreditsExit;
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private BlackScreenFader _blackScreenFader;

    public void HandleOnTimelineEnd()
    {
        FromSceneToScene fromSceneToScene;
        fromSceneToScene.from = "EndCreditsScene";
        fromSceneToScene.to = _homeScene;
        _onHomeSceneRequest.RaiseWithParam(fromSceneToScene);
    }

    public void ExitEnd()
    {
        _blackScreenFader.InitToMinEffect();
        _blackScreenFader.FadeIn();
        this.enabled = false;
    }

    public void EnterEnd()
    {
        _onFadeInEnd.OnEvent += HandleFadeInEnd;
        _blackScreenFader.InitToMaxEffect();
        _blackScreenFader.FadeOut();
        this.enabled = true;
    }

    private void HandleFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleFadeInEnd;

        FromSceneToScene fromSceneToScene;
        fromSceneToScene.from = "EndCreditsScene";
        fromSceneToScene.to = _homeScene;
        _onEndCreditsExit.RaiseWithParam(fromSceneToScene);
    }
}