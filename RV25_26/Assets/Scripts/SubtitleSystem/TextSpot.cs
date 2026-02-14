using UnityEngine;
using UnityEngine.Playables;

public struct SubtitleDataTimeReference
{
    public SubtitleData subtitleData;
    public AudioSource audioSource;
    public PlayableDirector playableDirector;
}

public class TextSpot : MonoBehaviour, ITextChangeable
{
    [SerializeField] private SubtitleData currentSubtitle;
    [SerializeField] private AppEventData _onSubtitleShow;
    [SerializeField] private AppEventData _onAudioStart;
    [SerializeField] private AppEventData _onAudioEnd;
    [Header("Se il tempo di riferimento è quello di un audio source, riempi il campo")]
    [SerializeField] private AudioSource _audioSource;
    [Header("Se il tempo di riferimento è quello di una timeline, riempi il campo")]
    [SerializeField] private PlayableDirector _playableDirector;
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
        SubtitleDataTimeReference st;
        st.audioSource = null;
        st.playableDirector = null;
        if(_audioSource != null) st.audioSource = _audioSource;
        if(_playableDirector != null) st.playableDirector = _playableDirector;
        st.subtitleData = currentSubtitle;

        if(!busy) _onSubtitleShow.RaiseWithParam(st);
    }
}