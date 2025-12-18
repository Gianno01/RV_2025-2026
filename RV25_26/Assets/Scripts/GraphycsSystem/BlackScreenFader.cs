
using DG.Tweening;
using UnityEngine;

public class BlackScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _blackScreen;
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private Ease _fadeInEase;
    [SerializeField] private float _fadeOutDuration;
    [SerializeField] private Ease _fadeOutEase;
    [SerializeField] private AppEventData _onEndFadeIn;
    [SerializeField] private AppEventData _onEndFadeOut;

    private Tween _fadeTween;
    
    public void FadeIn()
    {
        if(_fadeTween != null) _fadeTween.Kill();
        _fadeTween = DOTween.To(() => _blackScreen.alpha, (alpha) => _blackScreen.alpha = alpha, 1, _fadeInDuration).SetEase(_fadeInEase);
        _fadeTween.onComplete += () => _onEndFadeIn.Raise();
    }

    public void FadeOut()
    {
        if(_fadeTween != null) _fadeTween.Kill();
        _fadeTween = DOTween.To(() => _blackScreen.alpha, (alpha) => _blackScreen.alpha = alpha, 0, _fadeOutDuration).SetEase(_fadeOutEase);
        _fadeTween.onComplete += () => _onEndFadeOut.Raise();
    }
}
