using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SettingsMenu : MenuScreen
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private SaveData _settings;
    [SerializeField] private Text _resetText;
    
    private int _pressResetTimes = 6;

    protected override void OnEnable()
    {
        base.OnEnable();
        _pressResetTimes = 6;
        _resetText.text = "Reset Data";
    }

    public void MusicSliderChanged(float val)
    {
        _settings.SetMusicVolume(val);
        MusicManager.UpdateVolumeBasedOnSettings();
    }

    public void SoundSliderChanged(float val)
    {
        _settings.SetSoundVolume(val);
    }

    public void SaveSettings()
    {
        SaveManager.Save();
    }

    public void SyncUI()
    {
        _musicSlider.value = _settings.MusicVolume;
        _sfxSlider.value = _settings.SoundVolume;
    }

    public override void BtnPressBack()
    {
        base.BtnPressBack();
        SaveManager.Save();
    }

    public void BtnPressClearData()
    {
        _pressResetTimes --;

        if (_pressResetTimes > 0)
        {
            _resetText.text = $"press {_pressResetTimes} more times to confirm";
        }
        if (_pressResetTimes == 0)
        {
            _resetText.text = "resetting...";
            SaveManager.ResetSave();
            TransitionManager.LoadScene("TitleScene");
        }
    }
}
