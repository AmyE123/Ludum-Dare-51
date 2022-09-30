using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleRootMenu : MenuScreen
{
    [SerializeField]
    private TitleScreenManager _titleScreen;

    [SerializeField]
    private RectTransform _quitButton;

    [Header("Buttons"), SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _howToButton;    

    [SerializeField]
    private Button _settingsButton;

    private void Start()
    {
        _defaultSelectable = _playButton;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            _quitButton.gameObject.SetActive(false);
    }

    public void BtnPressPlay()
    {
        _titleScreen.ShowPlayScreen();
        _defaultSelectable = _playButton;
    }

    public void BtnPressInstructions()
    {
        _titleScreen.ShowInstructionsScreen();
        _defaultSelectable = _howToButton;
    }

    public void BtnPressSettings()
    {
        _titleScreen.ShowSettingsScreen();
        _defaultSelectable = _settingsButton;
    }

    public void BtnPressQuit()
    {
        Application.Quit();
    }
}
