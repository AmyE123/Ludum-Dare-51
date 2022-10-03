using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(-999)]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private SaveData _settings;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private float _baseVolume = 1;
    [SerializeField] private float _transitionTime = 1;
    [SerializeField] private AudioLowPassFilter _lowPassFilter;

    private static MusicManager _instance;

    public static float LowPassValue
    { 
        get => _instance._lowPassFilter.cutoffFrequency;
        set => _instance._lowPassFilter.cutoffFrequency = value;
    }

    public static float PitchValue
    { 
        get => _instance._musicPlayer.pitch;
        set => _instance._musicPlayer.pitch = value;
    }

    public static void ResetSelf()
    {
        _instance.StartCoroutine(_instance.ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        while (PitchValue < 1)
        {
            PitchValue = Mathf.Lerp(PitchValue, 1, Time.deltaTime * 3);
            LowPassValue = Mathf.Lerp(LowPassValue, 5000, Time.deltaTime * 3);
            yield return null;
        }

        Debug.Log("Restored!");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _musicPlayer.Play();

        UpdateVolumeBasedOnSettings();
        DontDestroyOnLoad(gameObject);

        _musicPlayer.volume = _baseVolume * _settings.MusicVolume;
    }

    public static void PlayMusic(AudioClip clip)
    {
        if (_instance._musicPlayer.clip == clip)
            return;

        float t = _instance._transitionTime / 2;
        float vol = _instance._settings.MusicVolume * _instance._baseVolume;
        AudioSource src = _instance._musicPlayer;

        src.DOFade(0, t).SetEase(Ease.Linear).OnComplete(() => 
        {
            src.clip = clip;
            src.Play();
            src.DOFade(vol, t).SetEase(Ease.Linear);
        });
    }

    public static void UpdateVolumeBasedOnSettings()
    {
        _instance._musicPlayer.volume = _instance._settings.MusicVolume * _instance._baseVolume;
    }
}
