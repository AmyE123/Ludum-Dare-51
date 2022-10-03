using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectCell : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Text _topText;

    [SerializeField]
    private Text _descText;

    [SerializeField]
    private Text _headerText;

    [SerializeField]
    private CanvasGroup _grp;

    [SerializeField]
    private RectTransform _contentRect;

    [SerializeField]
    private float _inactiveAlpha = 0.5f;

    [SerializeField]
    private float _inactiveSize = 0.9f;

    [SerializeField]
    private float _submitDownTime = 0.1f;

    [SerializeField]
    private float _submitDownSize = 0.95f;

    private ButtonEventForwarder _btn;
    private LevelData _level;
    private LevelSelectMenu _menu;

    private void Awake()
    {
        _btn = GetComponentInChildren<ButtonEventForwarder>();
        _btn.onSelect += OnSelect;
        _btn.onDeselect += OnDeselect;
        _btn.onPointerUp += OnClickUp;
        _btn.onPointerDown += OnClickDown;
        _btn.onSubmit += OnSubmit;

        _contentRect.localScale = new Vector3(_inactiveSize, _inactiveSize, _inactiveSize);
    }

    public void Init(LevelData level, LevelSelectMenu menu)
    {
        _level = level;
        _topText.text = level.LevelName;
        _descText.text = level.Description;
        _headerText.text = level.HeaderText;
        _menu = menu;
    }

    public void BtnPress()
    {
        _menu.ChooseLevel(_level);
    }

    private void OnSelect()
    {
        _grp.DOFade(1, 0.25f).SetEase(Ease.OutExpo);
        _contentRect.DOScale(1, 0.25f).SetEase(Ease.OutExpo);
    }

    private void OnDeselect()
    {
        _grp.DOFade(_inactiveAlpha, 0.25f).SetEase(Ease.OutExpo);
        _contentRect.DOScale(_inactiveSize, 0.25f).SetEase(Ease.OutExpo);
    }
    
    private void OnEnable()
    {
        _grp.alpha = _inactiveAlpha;
        _contentRect.localScale = new Vector3(_inactiveSize, _inactiveSize, _inactiveSize);
    }

    private void OnDisable()
    {
        _grp.alpha = _inactiveAlpha;
        _contentRect.localScale = new Vector3(_inactiveSize, _inactiveSize, _inactiveSize);
    }

    private void OnSubmit()
    {
        ChangeVisualsToDown();
        StartCoroutine(SubmitRoutine());
    }

    private void OnClickDown()
    {
        DOTween.Kill(_contentRect);
        StopAllCoroutines();
        ChangeVisualsToDown();
    }

    private void OnClickUp()
    {
        ChangeVisualsToUp();
    }

    private IEnumerator SubmitRoutine()
    {
        yield return new WaitForSeconds(_submitDownTime);
        ChangeVisualsToUp();
    }

    private void ChangeVisualsToDown()
    {
        _contentRect.DOScale(_submitDownSize, 0.1f);
    }

    private void ChangeVisualsToUp()
    {
        _contentRect.DOScale(1, 0.1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(_btn.gameObject);
    }
}
