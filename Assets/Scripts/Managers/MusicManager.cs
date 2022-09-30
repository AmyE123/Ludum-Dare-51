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

    private static MusicManager _instance;

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
