using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraPostProcessing : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;

    [SerializeField]
    private Camera _camera;

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

    [SerializeField, Range(5, 60)]
    private float _aliveFov = 42;

    [Header("Dying")]
    [SerializeField, Range(-100, 100)]
    private float _dyingSaturation;

    [SerializeField, Range(0, 1)]
    private float _dyingAbberation;

    [SerializeField, Range(0, 1)]
    private float _dyingVignetteIntensity;
    
    [SerializeField, Range(5, 60)]
    private float _deadFov = 42;

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

        float tgtFov = Mathf.Lerp(_aliveFov, _deadFov, percent);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, tgtFov, Time.deltaTime * 8);
    }
}
