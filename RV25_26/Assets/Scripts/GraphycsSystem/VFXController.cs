using DG.Tweening;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private BlackScreenFader _blackScreenFader;
    [SerializeField] private AppEventData _onEndFadeIn;
    [SerializeField] private AppEventData _onEndFadeOut;
    [SerializeField] private float _fadeInFadeOutSecGap;
    private bool _fadeOutAfterFadeIn = false;
    private bool _useFadeInFadeOutSecGap = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _onEndFadeIn.OnEvent += HandleOnFadeInEnd;
        _onEndFadeOut.OnEvent += HandleOnFadeOutEnd;
    }

    void OnDisable()
    {
        _onEndFadeIn.OnEvent -= HandleOnFadeInEnd;
        _onEndFadeOut.OnEvent -= HandleOnFadeOutEnd;
    }

    public void PlayChangeAreaFadeIn()
    {
        _fadeOutAfterFadeIn = true;
        _useFadeInFadeOutSecGap = true;
        _blackScreenFader.FadeIn();
    }

    private void HandleOnFadeInEnd()
    {
        if (_fadeOutAfterFadeIn)
        {
            if(_useFadeInFadeOutSecGap) DOVirtual.DelayedCall(_fadeInFadeOutSecGap, _blackScreenFader.FadeOut);
            else _blackScreenFader.FadeOut();  
        } 
    }

    private void HandleOnFadeOutEnd()
    {
        
    }
}
