using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraPostProcessing : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;

    private Vignette _vignette;
    private ColorAdjustments _colorAdjustments;
    private ChromaticAberration _chromaticAberration;

    [Header("Normal")]
    [SerializeField, Range(-100, 100)]
    private float _aliveSaturation;

    [SerializeField, Range(0, 1)]
    private float _aliveAbberation;

    [SerializeField, Range(0, 1)]
    private float _aliveVignetteIntensity;

    [Header("Dying")]
    [SerializeField, Range(-100, 100)]
    private float _dyingSaturation;

    [SerializeField, Range(0, 1)]
    private float _dyingAbberation;

    [SerializeField, Range(0, 1)]
    private float _dyingVignetteIntensity;

    private void Start()
    {
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _colorAdjustments);
        _volume.profile.TryGet(out _chromaticAberration);
    }

    public void SetDeadPercent(float percent)
    {
        _chromaticAberration.intensity.value = Mathf.Lerp(_aliveAbberation, _dyingAbberation, percent);
        _colorAdjustments.saturation.value = Mathf.Lerp(_aliveSaturation, _dyingSaturation, percent);
        _vignette.intensity.value = Mathf.Lerp(_aliveVignetteIntensity, _dyingVignetteIntensity, percent);
    }
}
