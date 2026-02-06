using UnityEngine;

public class SubtitlesController: MonoBehaviour
{
    [SerializeField] private AppEventData _onSubtitleShow;
    private SubtitleUI _subtitleUI;

    void Awake()
    {
        _onSubtitleShow.OnParamEvent += HandleOnSubtitlesShow;
    }

    void Start()
    {
        _subtitleUI = FindAnyObjectByType<SubtitleUI>();
    }

    void OnDisable()
    {
        _onSubtitleShow.OnParamEvent -= HandleOnSubtitlesShow;
    }

    private void HandleOnSubtitlesShow(object param)
    {
        SubtitleData subtitleData = (SubtitleData) param;

        _subtitleUI.Show(subtitleData.text);
    }
}