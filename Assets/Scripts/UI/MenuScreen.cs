using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class MenuScreen : MonoBehaviour
{
    [SerializeField]
    private ElementItem[] _animateInElements;

    [SerializeField]
    private ElementItem[] _animateOutElements;

    [SerializeField]
    protected Selectable _defaultSelectable;

    [SerializeField]
    private ScreenManager _screenManager;
    
    private CanvasGroup _grp;
    private RectTransform _rect;
    private HydroRobiaInput _input;

    protected void Awake()
    {
        _grp = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();
        _input = new HydroRobiaInput();
        _input.UI.Enable();
    }

    public virtual void BtnPressBack()
    {
        _screenManager?.GoBackOneLevel();
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(SelectDefaultSelectable());
    }

    private IEnumerator SelectDefaultSelectable()
    {
        yield return new WaitForSeconds(0.05f);
        
        if (_defaultSelectable)
            EventSystem.current.SetSelectedGameObject(_defaultSelectable.gameObject);
    }

    protected virtual void LateUpdate()
    {
        if (_defaultSelectable == null)
            return;

        Vector2 dirInput = _input.UI.Navigate.ReadValue<Vector2>();

        GameObject selected = EventSystem.current.currentSelectedGameObject;
        bool isSelectedNull = selected == null || selected.activeInHierarchy == false;

        if (dirInput.magnitude > 0.1f && isSelectedNull)
        {
            EventSystem.current.SetSelectedGameObject(_defaultSelectable.gameObject);
        }
    }

    public void SnapToInactive()
    {
        DOTween.Kill(_rect);
        DOTween.Kill(_grp);

        foreach (ElementItem item in _animateInElements)
            item.element.SnapToInactive();
    }

    public void MoveOffScreen(Vector2 newPos, float timing, Ease ease)
    {
        DOTween.Kill(_rect);
        DOTween.Kill(_grp);

        _grp.interactable = _grp.blocksRaycasts = false;

        _rect.DOAnchorPos(newPos, timing).SetEase(ease);
        _grp.DOFade(0, timing).SetEase(ease).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void MoveOntoScreen(Vector2 fromPos, float timing, float delay, Ease ease)
    {
        DOTween.Kill(_rect);
        DOTween.Kill(_grp);

        gameObject.SetActive(true);
        _rect.anchoredPosition = fromPos;
        _rect.DOAnchorPos(Vector2.zero, timing).SetDelay(delay).SetEase(ease);
        _grp.interactable = _grp.blocksRaycasts = true;

        _grp.alpha = 0;
        _grp.DOFade(1, timing).SetDelay(delay).SetEase(ease);
    }

    public void AnimateIn(float delay=0)
    {
        foreach (ElementItem item in _animateInElements)
            item.element.AnimateIn(item.delay + delay);
    }

    public void AnimateOut(float delay=0)
    {
        foreach (ElementItem item in _animateOutElements)
            item.element.AnimateOut(item.delay + delay);
    }

    [System.Serializable]
    public class ElementItem
    {
        public MenuScreenElement element;
        public float delay;
    }
}
