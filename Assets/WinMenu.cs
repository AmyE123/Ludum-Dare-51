using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class WinMenu : MenuScreen
{
    [SerializeField]
    private CanvasGroup _rootCanvasGroup;

    [SerializeField]
    private LevelList _levels;

    [SerializeField]
    private AdvancedButton _nextLevelButton;

    [SerializeField]
    private AdvancedButton _mainMenuButton;

    private bool _isOnScreen;

    public bool IsOnScreen => _isOnScreen;

    void Start()
    {
        if (_levels.IsLastLevel)
        {
            _nextLevelButton.gameObject.SetActive(false);
            _defaultSelectable = _mainMenuButton.GetComponentInChildren<Button>();
        }
    }

    public void SnapClosed()
    {
        _isOnScreen = false;
        _rootCanvasGroup.gameObject.SetActive(false);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        SnapToInactive();
    }

    public void ShowWinMenu()
    {
        _isOnScreen = true;
        _rootCanvasGroup.gameObject.SetActive(true);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = true;
        _rootCanvasGroup.DOFade(1, 0.15f);
        AnimateIn();
    }

    private void HideWinMenu()
    {
        _isOnScreen = false;
        DOTween.Kill(_rootCanvasGroup);
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        AnimateOut();

        _rootCanvasGroup.DOFade(0, 0.3f).SetDelay(0.5f).OnComplete(() => _rootCanvasGroup.gameObject.SetActive(false));
    }

    public void BtnPressMenu()
    {
        TransitionManager.LoadScene("TitleScene");
    }

    public void BtnPressContinue()
    {
        _levels.IncrementLevel();
        TransitionManager.LoadScene("GameScene");
    }
}
