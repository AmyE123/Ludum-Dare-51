using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [SerializeField]
    private SaveData _settings;

    private static Camera _mainCam;
    private static SfxManager _instance;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary> Clip only (it will instantiate at the Camera's position) </summary>
    public static void PlaySound(AudioClip clip, float volume=1, float pitch=1)
    {
        if (_mainCam == null)
            _mainCam = Camera.main;

        PlaySound(clip, _mainCam.transform.position, volume, pitch);
    }

    /// <summary> Clip + Position </summary>
    public static void PlaySound(AudioClip clip, Vector3 position, float volume=1, float pitch=1)
    {
        if (_instance._settings.SoundVolume <= 0)
            return;

        GameObject newObj = new GameObject($"SoundEffect({clip.name})");
        newObj.transform.position = position;

        AudioSource source = newObj.AddComponent<AudioSource>();
        source.volume = volume * _instance._settings.SoundVolume;
        source.pitch = pitch;
        source.PlayOneShot(clip);

        Destroy(newObj, (clip.length / pitch) + 0.1f);
    }

    /// <summary> Clip + Transform </summary>
    public static void PlaySound(AudioClip clip, Transform position, float volume=1, float pitch=1)
    {
        PlaySound(clip, position.position, volume, pitch);
    }

    /// <summary> Clip only (it will instantiate at the Camera's position) </summary>
    public static void PlaySoundRandomPitch(AudioClip clip, float minPitch, float maxPitch, float volume=1)
    {
        if (_mainCam == null)
            _mainCam = Camera.main;

        float pitch = Random.Range(minPitch, maxPitch);
        PlaySound(clip, _mainCam.transform.position, volume, pitch);
    }

    /// <summary> Clip + Position </summary>
    public static void PlaySoundRandomPitch(AudioClip clip, Vector3 position, float minPitch, float maxPitch, float volume=1)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        PlaySound(clip, position, volume, pitch);
    }

    /// <summary> Clip + Tranform </summary>
    public static void PlaySoundRandomPitch(AudioClip clip, Transform position, float minPitch, float maxPitch, float volume=1)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        PlaySound(clip, position.position, volume, pitch);
    }

}
