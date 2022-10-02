using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DeathMenu : MenuScreen
{
    [SerializeField]
    private CanvasGroup _rootCanvasGroup;

    public void SnapClosed()
    {
        _rootCanvasGroup.gameObject.SetActive(false);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        SnapToInactive();
    }

    public void ShowDeathMenu()
    {
        _rootCanvasGroup.gameObject.SetActive(true);
        _rootCanvasGroup.alpha = 0;
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = true;
        _rootCanvasGroup.DOFade(1, 0.15f);
        AnimateIn();
    }

    private void HideDeathMenu()
    {
        DOTween.Kill(_rootCanvasGroup);
        _rootCanvasGroup.blocksRaycasts = _rootCanvasGroup.interactable = false;
        AnimateOut();

        _rootCanvasGroup.DOFade(0, 0.3f).SetDelay(0.5f).OnComplete(() => _rootCanvasGroup.gameObject.SetActive(false));
    }

    public void BtnPressMenu()
    {
        TransitionManager.LoadScene("TitleScene");
    }

    public void BtnPressRetry()
    {
        TransitionManager.LoadScene("GameScene");
    }
}
