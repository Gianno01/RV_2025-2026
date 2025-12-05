using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableAsset[] _timelines;
    [SerializeField] private AppEventData _onTimelineEnd;
    private PlayableDirector _playableDirector;
    private int _currentTimelineIndex;

    //da far chiamare all'appcontroller, una volta eseguito il cambio di stato e attivato il CutsceneController
    public void PlayTimeline(int timelineIndex)
    {
        _currentTimelineIndex = timelineIndex;
        _playableDirector.playableAsset = _timelines[_currentTimelineIndex];
        _onTimelineEnd.OnEvent += HandleOnTimelineEnd;
        _playableDirector.Play();
    }

    private void HandleOnTimelineEnd()
    {
        switch (_currentTimelineIndex)
        {
            case 0:
                break;
            //ecc. altri case
        }
        _onTimelineEnd.OnEvent -= HandleOnTimelineEnd;
    }
}