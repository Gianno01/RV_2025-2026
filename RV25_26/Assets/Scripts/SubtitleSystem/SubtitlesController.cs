using System.Collections;
using UnityEngine;

public class SubtitlesController: MonoBehaviour
{
    [SerializeField] private AppEventData _onSubtitleShow;
    //[SerializeField] private AppEventData _onTextEnd;
    [SerializeField] private bool _stopCurrentSubtitle;
    private bool _busyWithASubtitle = false;
    private SubtitleUI _subtitleUI;
    private Coroutine _subtitleCoroutine;

    void Awake()
    {
        _onSubtitleShow.OnParamEvent += HandleOnSubtitlesShow;
        //_onTextEnd.OnEvent += HandleOnTextEnd;
    }

    void Start()
    {
        _subtitleUI = FindAnyObjectByType<SubtitleUI>();
    }

    void OnDisable()
    {
        _onSubtitleShow.OnParamEvent -= HandleOnSubtitlesShow;
        //_onTextEnd.OnEvent -= HandleOnTextEnd;
    }

    /*private void HandleOnTextEnd()
    {
        _currentSubtitleData = null;
    }*/

    private void HandleOnSubtitlesShow(object param)
    {
        SubtitleDataTimeReference st = (SubtitleDataTimeReference) param;

        if(_busyWithASubtitle && !_stopCurrentSubtitle) return;
        _busyWithASubtitle = true;

        if(st.subtitleData != null)
        {
            if(_subtitleCoroutine != null)
            {
                StopCoroutine(_subtitleCoroutine);
                _subtitleUI.CloseUI();
            }
            _subtitleCoroutine = StartCoroutine(PlaySubtitles(st));
        } 
    }

    private IEnumerator PlaySubtitles(SubtitleDataTimeReference st)
    {
        float timer = 0;
        timer = UpdateTimer(timer, st);
        
        _subtitleUI.OpenUI();
        foreach(SubtitleChunk chunk in st.subtitleData.clipSubtitleChunks)
        {
            while (timer < chunk.startTime)
            {
                yield return null;
                timer = UpdateTimer(timer, st);
            }

            _subtitleUI.Show(chunk.text);

            while (timer < chunk.endTime)
            {
                yield return null;
                timer = UpdateTimer(timer, st);
            }
        }
        _subtitleUI.CloseUI();
        _busyWithASubtitle = false;
    }

    private float UpdateTimer(float startTimer, SubtitleDataTimeReference st)
    {
        if(st.audioSource != null && !(st.audioSource.clip == null) && !(st.audioSource.time == 0 && startTimer > 0)) return st.audioSource.time;
        if(st.playableDirector != null && !(st.playableDirector.playableAsset == null) && !(st.playableDirector.time == 0 && startTimer > 0)) return (float) st.playableDirector.time;
        return startTimer += Time.deltaTime;
    }
}