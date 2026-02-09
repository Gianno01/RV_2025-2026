using UnityEngine;

public class TextSpot : MonoBehaviour, ITextChangeable
{
    [SerializeField] private SubtitleData currentSubtitle;
    [SerializeField] private AppEventData _onSubtitleShow;
    [SerializeField] private AppEventData _onAudioStart;
    [SerializeField] private AppEventData _onAudioEnd;
    private bool busy = false;

    void OnEnable()
    {
        if(_onAudioStart != null) _onAudioStart.OnEvent += HandleOnAudioStart;
        if(_onAudioEnd != null) _onAudioEnd.OnEvent += HandleOnAudioEnd;
    }

    void OnDisable()
    {
        if(_onAudioStart != null) _onAudioStart.OnEvent -= HandleOnAudioStart;
        if(_onAudioEnd != null) _onAudioEnd.OnEvent -= HandleOnAudioEnd;
    }

    private void HandleOnAudioStart()
    {
        busy = true;
    }

    private void HandleOnAudioEnd()
    {
        busy = false;
    }

    public void ChangeText(SubtitleData text)
    {
        currentSubtitle = text;
    }

    public void ShowText()
    {
        if(!busy) _onSubtitleShow.RaiseWithParam(currentSubtitle);
    }
}