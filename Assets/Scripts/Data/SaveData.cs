using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private string _saveKey = "SaveData";

    [Space(12)]
    [SerializeField, Range(0, 1)]
    private float _soundVolume;
    
    [SerializeField, Range(0, 1)]
    private float _musicVolume;

    [SerializeField]
    private List<string> _completedLevels = new List<string>();

    public float SoundVolume => _soundVolume;
    public float MusicVolume => _musicVolume;

    public void SetSoundVolume(float val) => _soundVolume = val;
    public void SetMusicVolume(float val) => _musicVolume = val;

    public bool HasCompletedLevel(LevelData level) => _completedLevels.Contains(level.name);

    public void CompleteLevel(LevelData level)
    {
        if (_completedLevels.Contains(level.name))
            return;

        _completedLevels.Add(level.name);
        SaveToPrefs();
    }

    public void SaveToPrefs()
    {
        string jstring = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(_saveKey, jstring);
        PlayerPrefs.Save();
    }

    public void LoadFromPrefs()
    {
        if (PlayerPrefs.HasKey(_saveKey) == false)
            return;

        string jstring = PlayerPrefs.GetString(_saveKey);
        JsonUtility.FromJsonOverwrite(jstring, this);
    }

    public bool HasSavedData => PlayerPrefs.HasKey(_saveKey);
}
