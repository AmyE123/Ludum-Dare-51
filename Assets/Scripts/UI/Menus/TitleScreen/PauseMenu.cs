using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MenuScreen
{
    [SerializeField]
    private CanvasGroup _rootCanvasGroup;

    private bool _isVisible;

    private static bool _isGamePaused;

    public static bool IsGamePaused => _isGamePaused;

    public void ToggleMenu()
    {
        if (_isVisible)
            HidePauseMenu();
        else
            ShowPauseMenu();
    }

    public void SnapClosed()
    {
        _rootCanvasGroup.gameObject.SetActive(false);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        SnapToInactive();

        _isVisible = false;
        _isGamePaused = false;
    }

    private IEnumerator UnpauseGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isGamePaused = false;
    }

    public void ShowPauseMenu()
    {
        _isVisible = true;
        _isGamePaused = true;

        DOTween.Kill(_rootCanvasGroup);
        _rootCanvasGroup.gameObject.SetActive(true);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = true;
        _rootCanvasGroup.DOFade(1, 0.15f);
        AnimateIn();
    }

    private void HidePauseMenu()
    {
        _isVisible = false;
        StartCoroutine(UnpauseGame(0.1f));

        DOTween.Kill(_rootCanvasGroup);
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        AnimateOut();

        _rootCanvasGroup.DOFade(0, 0.3f).SetDelay(0.5f).OnComplete(() => _rootCanvasGroup.gameObject.SetActive(false));
    }

    public void BtnPressPlay()
    {
        HidePauseMenu();
    }

    public void BtnPressMenu()
    {
        TransitionManager.LoadScene("MenuScene");
    }

    public void BtnPressRetry()
    {
        TransitionManager.LoadScene("GameScene");
    }
}
