using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VFXController : MonoBehaviour
{
    [SerializeField] private Camera _VFXCamera;

    [SerializeField] private BlackScreenFader _blackScreenFader;
    [SerializeField] private float _fadeInOutSecGap;
    [SerializeField] private AppEventData _onFadeInEnd;
    [SerializeField] private AppEventData _onFadeOutEnd;

    [SerializeField] private ExplosionFader _explosionFader;
    [SerializeField] private float _explosionInOutSecGap;
    [SerializeField] private AppEventData _onExplosionInEnd;
    [SerializeField] private AppEventData _onExplosionOutEnd;

    [SerializeField] private ExplosionFader _wakeUpFader;
    [SerializeField] private AppEventData _onWakeUpOutEnd;

    private bool _OutAfterIn = false;
    private bool _useInOutSecGap = false;

    private UniversalAdditionalCameraData mainCam;

    void Start()
    {
        mainCam = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        mainCam.cameraStack.Add(_VFXCamera);

        _blackScreenFader.InitToMaxEffect();
        _wakeUpFader.InitToMaxEffects();

        _onFadeInEnd.OnEvent += HandleOnFadeInEnd;
        _onFadeOutEnd.OnEvent += HandleOnFadeOutEnd;
        _onExplosionInEnd.OnEvent += HandleOnExplosionInEnd;
        _onExplosionOutEnd.OnEvent += HandleOnExplosionOutEnd;
        _onWakeUpOutEnd.OnEvent += HandleOnWakeUpOutEnd;
    }

    void OnDisable()
    {
        _onFadeInEnd.OnEvent -= HandleOnFadeInEnd;
        _onFadeOutEnd.OnEvent -= HandleOnFadeOutEnd;
        _onExplosionInEnd.OnEvent -= HandleOnExplosionInEnd;
        _onExplosionOutEnd.OnEvent -= HandleOnExplosionOutEnd;
        _onWakeUpOutEnd.OnEvent -= HandleOnWakeUpOutEnd;
    }

    public void PlayChangeAppStateFadeIn()
    {
        _OutAfterIn = false;
        _useInOutSecGap = false;
        _blackScreenFader.InitToMinEffect();
        _blackScreenFader.FadeIn();
    }

    public void PlayChangeAppStateFadeOut()
    {
        _blackScreenFader.InitToMaxEffect();
        _blackScreenFader.FadeOut();
    }

    public void PlayChangeAreaFadeIn()
    {
        _OutAfterIn = true;
        _useInOutSecGap = true;
        _blackScreenFader.InitToMinEffect();
        _blackScreenFader.FadeIn();
    }

    public void PlayExplosionIn()
    {
        _OutAfterIn = false;
        _useInOutSecGap = false;
        _explosionFader.InitToMinEffects();
        _explosionFader.ExplodeIn();
    }

    public void PlayWakeUpOut()
    {
        _OutAfterIn = false;
        _useInOutSecGap = false;
        _blackScreenFader.InitToMaxEffect();
        _wakeUpFader.InitToMaxEffects();
        _blackScreenFader.FadeOut();
        _wakeUpFader.ExplodeOut();
    }

    private void HandleOnFadeInEnd()
    {
        if (_OutAfterIn)
        {
            if(_useInOutSecGap) DOVirtual.DelayedCall(_fadeInOutSecGap, () => {_blackScreenFader.InitToMaxEffect(); _blackScreenFader.FadeOut();});
            else _blackScreenFader.FadeOut();  
        }
    }

    private void HandleOnFadeOutEnd()
    {
        _blackScreenFader.InitToMinEffect();
    }

    private void HandleOnExplosionInEnd()
    {
        // per test. In realtà c'è un cambio di scena e nella nuova scena initMaxEffect ed explode out
        if (_OutAfterIn)
        {
            if(_useInOutSecGap) DOVirtual.DelayedCall(_explosionInOutSecGap, () => {_explosionFader.InitToMaxEffects(); _explosionFader.ExplodeOut();});
            else {_explosionFader.InitToMaxEffects(); _explosionFader.ExplodeOut();}  
        } 
    }

    private void HandleOnExplosionOutEnd()
    {
        _explosionFader.DisableEffects();
    }

    private void HandleOnWakeUpOutEnd()
    {
        _wakeUpFader.DisableEffects();
    }
}
