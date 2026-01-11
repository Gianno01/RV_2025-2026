using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// Importante avere una camera (VFXCamera nei prefab) in overlay da aggiungere allo stack della camera base.
/// Solo questa camera renderizza il VFXPostProcessingCanvas (nei prefab).
/// Non toccare i parametri dell'effetto bloom. Per cambiare la brillantezza dell'esplosione si usa l'intensità di un colore HDR. 
/// Per farlo funzionare serve comunque un bloom attivo e preimpostato. Si trova insieme agli altri effetti nel prefab VFXVolume.
/// Prima di chiamare ExplodeIn e ExplodeOut è bene inizializzare gli effetti con InitToMinEffects e InitToMaxEffects.
/// Al termine di ExplodeOut è possibile che si voglia disattivare gli effetti con DisableEffects.
/// </summary>
public class ExplosionFader : MonoBehaviour
{
    [SerializeField] private Volume _screenVfxVolume;
    [SerializeField] private Image _whitePoint;

    [SerializeField] private Ease _scaleInEase;
    [SerializeField] private Ease _scaleOutEase;
    [SerializeField] private Vector3 _minPointScale;
    [SerializeField] private Vector3 _maxPointScale;
    [SerializeField] private float _scaleInDuration;
    [SerializeField] private float _scaleOutDuration;

    [SerializeField] [ColorUsage(true, true)] private Color _baseColor;
    [SerializeField] private float _minHDRIntensity;
    [SerializeField] private float _maxHDRIntensity;
    [SerializeField] private float _bloomInDuration;
    [SerializeField] private float _bloomOutDuration;

    [SerializeField] private float _minFocusDistance;
    [SerializeField] private float _maxFocusDistance;
    [SerializeField] private float _depthOfFieldInDuration;
    [SerializeField] private float _depthOfFieldOutDuration;

    [SerializeField] private float _minDistortionIntensity;
    [SerializeField] private float _maxDistortionIntensity;
    [SerializeField] private float _lensDistortionInDuration;
    [SerializeField] private float _lensDistortionOutDuration;

    [SerializeField] private float _minAberationIntensity;
    [SerializeField] private float _maxAberrationIntensity;
    [SerializeField] private float _chromaticAberrationInDuration;
    [SerializeField] private float _chromaticAberrationOutDuration;

    [SerializeField] private AppEventData _onExplosionInEnd;
    [SerializeField] private AppEventData _onExplosionOutEnd;

    private Bloom _bloom;
    private DepthOfField _depthOfField;
    private LensDistortion _lensDistortion;
    private ChromaticAberration _chromaticAberration;
    private Material _whitePointMaterial;

    private Sequence _sequence;

    void Start()
    {
        VolumeProfile profile = _screenVfxVolume.profile;
        profile.TryGet(out _bloom);
        profile.TryGet(out _depthOfField);
        profile.TryGet(out _lensDistortion);
        profile.TryGet(out _chromaticAberration);

        _bloom.active = false;
        _depthOfField.active = false;
        _lensDistortion.active = false;
        _chromaticAberration.active = false;

        _depthOfField.focusDistance.overrideState = true;
        _lensDistortion.intensity.overrideState = true;
        _chromaticAberration.intensity.overrideState = true;

        _whitePointMaterial = _whitePoint.material;
        _whitePointMaterial.SetColor("_EmissionColor", _baseColor);
    }

    public void InitToMinEffects()
    {
        float factor = Mathf.Pow(2,_minHDRIntensity);
        Color minColor = new Color(_baseColor.r * factor, _baseColor.g * factor, _baseColor.b * factor, _baseColor.a);
        _whitePoint.transform.localScale = _minPointScale;
        _whitePointMaterial.SetColor("_EmissionColor", minColor);
        _depthOfField.focusDistance.value = _maxFocusDistance;
        _lensDistortion.intensity.value = _minDistortionIntensity;
        _chromaticAberration.intensity.value = _minAberationIntensity;

        EnableEffects();
    }

    public void InitToMaxEffects()
    {
        float factor = Mathf.Pow(2,_maxHDRIntensity);
        Color maxColor = new Color(_baseColor.r * factor, _baseColor.g * factor, _baseColor.b * factor, _baseColor.a);
        _whitePoint.transform.localScale = _maxPointScale;
        _whitePointMaterial.SetColor("_EmissionColor", maxColor);
        _depthOfField.focusDistance.value = _minFocusDistance;
        _lensDistortion.intensity.value = _maxDistortionIntensity;
        _chromaticAberration.intensity.value = _maxAberrationIntensity;

        EnableEffects();
    }

    public void EnableEffects()
    {
        _whitePoint.enabled = true;
        _bloom.active = true;
        _depthOfField.active = true;
        _lensDistortion.active = true;
        _chromaticAberration.active = true;
    }

    public void DisableEffects()
    {
        _whitePoint.enabled = false;
        _bloom.active = false;
        _depthOfField.active = false;
        _lensDistortion.active = false;
        _chromaticAberration.active = false;
    }

    public void ExplodeIn()
    {
        if(_sequence != null) _sequence.Kill();

        float factor = Mathf.Pow(2,_maxHDRIntensity);
        Color maxColor = new Color(_baseColor.r * factor, _baseColor.g * factor, _baseColor.b * factor, _baseColor.a);

        _sequence = DOTween.Sequence();
        _sequence.Append(DOTween.To(() => _whitePoint.transform.localScale, (s) => _whitePoint.transform.localScale = s, _maxPointScale, _scaleInDuration)).SetEase(_scaleInEase);
        _sequence.Join(DOTween.To(() => _whitePointMaterial.GetColor("_EmissionColor"), (c) => _whitePointMaterial.SetColor("_EmissionColor", c), maxColor, _bloomInDuration));
        _sequence.Join(DOTween.To(() => _depthOfField.focusDistance.value, (f) => _depthOfField.focusDistance.value = f, _minFocusDistance, _depthOfFieldInDuration));
        _sequence.Join(DOTween.To(() => _lensDistortion.intensity.value, (d) => _lensDistortion.intensity.value = d, _maxDistortionIntensity, _lensDistortionInDuration));
        _sequence.Join(DOTween.To(() => _chromaticAberration.intensity.value, (a) => _chromaticAberration.intensity.value = a, _maxAberrationIntensity, _chromaticAberrationInDuration));
        if(_onExplosionInEnd != null) _sequence.onComplete += () => _onExplosionInEnd.Raise();
        _sequence.Play();
    }

    public void ExplodeOut()
    {
        if(_sequence != null) _sequence.Kill();

        float factor = Mathf.Pow(2,_minHDRIntensity);
        Color minColor = new Color(_baseColor.r * factor, _baseColor.g * factor, _baseColor.b * factor, _baseColor.a);

        _bloom.active = true;
        _depthOfField.active = true;
        _lensDistortion.active = true;
        _chromaticAberration.active = true;

        _sequence = DOTween.Sequence();
        _sequence.Append(DOTween.To(() => _whitePoint.transform.localScale, (s) => _whitePoint.transform.localScale = s, _minPointScale, _scaleOutDuration)).SetEase(_scaleOutEase);
        _sequence.Join(DOTween.To(() => _whitePointMaterial.GetColor("_EmissionColor"), (c) => _whitePointMaterial.SetColor("_EmissionColor", c), minColor, _bloomOutDuration));
        _sequence.Join(DOTween.To(() => _depthOfField.focusDistance.value, (f) => _depthOfField.focusDistance.value = f, _maxFocusDistance, _depthOfFieldOutDuration));
        _sequence.Join(DOTween.To(() => _lensDistortion.intensity.value, (d) => _lensDistortion.intensity.value = d, _minDistortionIntensity, _lensDistortionOutDuration));
        _sequence.Join(DOTween.To(() => _chromaticAberration.intensity.value, (a) => _chromaticAberration.intensity.value = a, _minAberationIntensity, _chromaticAberrationOutDuration));
        if(_onExplosionOutEnd != null) _sequence.onComplete += () => { _onExplosionOutEnd.Raise();};
        _sequence.Play();
    }
}
