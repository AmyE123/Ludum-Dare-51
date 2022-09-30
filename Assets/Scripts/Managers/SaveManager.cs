using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;

    [SerializeField]
    private SaveData _activeSave;

    [SerializeField]
    private SaveData _defaultSave;

    public static SaveData SaveData => _instance._activeSave;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
        Load();
    }

    public static void Save()
    {
        _instance._activeSave.SaveToPrefs();
    }

    public static void Load()
    {
        if (_instance._activeSave.HasSavedData == false)
        {
            ResetSave();
            return;
        }

        _instance._activeSave.LoadFromPrefs();
    }

    public static void ResetSave()
    {
        _instance.ActuallyResetSave();
    }

    private void ActuallyResetSave()
    {
        string jstring = JsonUtility.ToJson(_defaultSave);
        JsonUtility.FromJsonOverwrite(jstring, _activeSave);
        _activeSave.SaveToPrefs();
    }

    #if UNITY_EDITOR
    public void ResetSaveEditor() => ActuallyResetSave();

    public void ResetSaveWithoutPlayerPrefs()
    {
        string jstring = JsonUtility.ToJson(_defaultSave);
        JsonUtility.FromJsonOverwrite(jstring, _activeSave);
    }
    #endif
}
