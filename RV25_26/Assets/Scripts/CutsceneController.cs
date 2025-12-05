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
    private PlayableDirector _playableDirector;
    private int _currentTimelineIndex;

    void Awake()
    {
        _playableDirector = gameObject.GetComponent<PlayableDirector>();
    }

    public void PlayTimeline(int timelineIndex)
    {
        _currentTimelineIndex = timelineIndex;
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
        _onCutsceneStart.RaiseWithParam(timelineIndex);
    }
}