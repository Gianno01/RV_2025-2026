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

    private bool _OutAfterIn = false;
    private bool _useInOutSecGap = false;

    private UniversalAdditionalCameraData mainCam;

    void Start()
    {
        mainCam = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        mainCam.cameraStack.Add(_VFXCamera);

        _onFadeInEnd.OnEvent += HandleOnFadeInEnd;
        _onFadeOutEnd.OnEvent += HandleOnFadeOutEnd;
        _onExplosionInEnd.OnEvent += HandleOnExplosionInEnd;
        _onExplosionOutEnd.OnEvent += HandleOnExplosionOutEnd;
    }

    void OnDisable()
    {
        _onFadeInEnd.OnEvent -= HandleOnFadeInEnd;
        _onFadeOutEnd.OnEvent -= HandleOnFadeOutEnd;
        _onExplosionInEnd.OnEvent -= HandleOnExplosionInEnd;
        _onExplosionOutEnd.OnEvent -= HandleOnExplosionOutEnd;
    }

    public void PlayChangeAreaFadeIn()
    {
        _OutAfterIn = true;
        _useInOutSecGap = true;
        _blackScreenFader.FadeIn();
    }

    public void PlayExplosionIn()
    {
        _OutAfterIn = true;
        _useInOutSecGap = true;
        _explosionFader.InitToMinEffects();
        _explosionFader.ExplodeIn();
    }

    private void HandleOnFadeInEnd()
    {
        if (_OutAfterIn)
        {
            if(_useInOutSecGap) DOVirtual.DelayedCall(_fadeInOutSecGap, _blackScreenFader.FadeOut);
            else _blackScreenFader.FadeOut();  
        } 
    }

    private void HandleOnFadeOutEnd()
    {
        // nulla per ora
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
}
