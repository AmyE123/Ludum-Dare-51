using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScreenManager : ScreenManager
{
    [Header("Screens")]
    [SerializeField] private MenuScreen _rootScreen;
    [SerializeField] private MenuScreen _playScreen;
    [SerializeField] private MenuScreen _instructionsScreen;
    [SerializeField] private MenuScreen _settingsScreen;

    [Header("More Timing")]
    [Tooltip("How long after scene load should we wait before showing the root menu?")]
    [SerializeField] private float _initialLoadDelay = 0.5f;

    void Start()
    {
        InitStack(_rootScreen);
        _rootScreen.SnapToInactive();

        _rootScreen.gameObject.SetActive(true);
        _playScreen.gameObject.SetActive(false);
        _instructionsScreen.gameObject.SetActive(false);
        _settingsScreen.gameObject.SetActive(false);

        StartCoroutine(InitialShowRootScreen());
    }

    IEnumerator InitialShowRootScreen()
    {
        yield return new WaitForSeconds(_initialLoadDelay);
        _rootScreen.AnimateIn();
    }


    public void ShowPlayScreen() => ShowScreen(_playScreen);

    public void ShowInstructionsScreen() => ShowScreen(_instructionsScreen);

    public void ShowSettingsScreen() => ShowScreen(_settingsScreen);
}
