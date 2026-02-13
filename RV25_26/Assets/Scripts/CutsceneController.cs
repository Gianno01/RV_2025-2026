using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// CutsceneController imposta la timeline corrente e la esegue con il metodo PlayTimeline invocato dall'AppController
/// all'evento _onCutsceneStart. Alla fine della timeline si aspetta che venga invocato un signal,
/// così da richiamare la callback HandleOnTimelineEnd.
/// </summary>
public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableAsset[] _timelines;
    //[SerializeField] private AppEventData _onTimelineEnd;
    [SerializeField] private AppEventData _onGameplayResume;
    [SerializeField] private AppEventData _onCutsceneStart;
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private AppEventData _onCutsceneExit;
    private PlayableDirector _playableDirector;
    private int _currentTimelineIndex;
    private VFXController _VFXController;

    void Awake()
    {
        _playableDirector = gameObject.GetComponent<PlayableDirector>();
    }

    public void Init()
    {
        _VFXController = GameObject.FindAnyObjectByType<VFXController>();
        this.enabled = false;
    }

    public void PlayCurrentTimeline()
    {
        _playableDirector.playableAsset = _timelines[_currentTimelineIndex];
        //_onTimelineEnd.OnEvent += HandleOnTimelineEnd;
        _playableDirector.Play();
    }

    public void HandleOnTimelineEnd()
    {
        /*switch (_currentTimelineIndex)
        {
            case 0:
                break;
            //ecc. altri case
        }*/
        _onGameplayResume.Raise();
        //_onTimelineEnd.OnEvent -= HandleOnTimelineEnd;
    }

    public void StartCutscene(int timelineIndex)
    {
        _currentTimelineIndex = timelineIndex;
        _onCutsceneStart.Raise();
    }

    public void EnterCutscene()
    {
        this.enabled = true;
        _VFXController.PlayChangeAppStateFadeOut();
    }

    public void ExitCutscene()
    {
        _onFadeInEnd.OnEvent += HandleOnFadeInEnd;
        _VFXController.PlayChangeAppStateFadeIn();
    }

    private void HandleOnFadeInEnd()
    {
        _onFadeInEnd.OnEvent -= HandleOnFadeInEnd;
        /*GameObject cutsceneCam = GameObject.FindAnyObjectByType<CinemachineCamera>().gameObject;
        cutsceneCam.SetActive(false);*/
        _onCutsceneExit.Raise();
        this.enabled = false;
    }
}