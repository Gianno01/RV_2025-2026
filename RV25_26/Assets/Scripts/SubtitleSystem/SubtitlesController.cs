using UnityEngine;

public class SubtitlesController: MonoBehaviour
{
    [SerializeField] private AppEventData _onSubtitleShow;
    [SerializeField] private AppEventData _onTextEnd;
    private SubtitleData _currentSubtitleData;
    private SubtitleUI _subtitleUI;

    void Awake()
    {
        _onSubtitleShow.OnParamEvent += HandleOnSubtitlesShow;
        _onTextEnd.OnEvent += HandleOnTextEnd;
    }

    void Start()
    {
        _subtitleUI = FindAnyObjectByType<SubtitleUI>();
    }

    void OnDisable()
    {
        _onSubtitleShow.OnParamEvent -= HandleOnSubtitlesShow;
        _onTextEnd.OnEvent -= HandleOnTextEnd;
    }

    private void HandleOnTextEnd()
    {
        _currentSubtitleData = null;
    }

    private void HandleOnSubtitlesShow(object param)
    {
        SubtitleData subtitleData = (SubtitleData) param;

        if(_currentSubtitleData != null && _currentSubtitleData == subtitleData) return;

        _currentSubtitleData = subtitleData;
        if(subtitleData != null) _subtitleUI.Show(subtitleData.text);
    }
}