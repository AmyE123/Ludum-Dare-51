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

    private void Start()
    {
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _colorAdjustments);
        _volume.profile.TryGet(out _chromaticAberration);
    }

    public void UpdatePostProcessing()
    {
        _chromaticAberration.intensity.value += Time.deltaTime;
        _colorAdjustments.saturation.value -= Time.deltaTime * 30;

        if (_vignette.intensity.value <= 0.3f)
        {
            _vignette.intensity.value += Time.deltaTime / 30;
        }
    }
}
