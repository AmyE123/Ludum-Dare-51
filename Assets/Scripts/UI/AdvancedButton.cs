using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class AdvancedButton : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _upSprite;
    
    [SerializeField]
    private Sprite _downSprite;

    [Header("Outline")]
    [SerializeField]
    private Image _selectionOutline;

    [SerializeField]
    private Color _notSelectedColor;

    [SerializeField]
    private Color _isSelectedColor;

    [Header("Submitting")]
    [SerializeField]
    private float _submitDownTime = 0.2f;
    
    [SerializeField]
    private RectTransform _contentRect;

    [SerializeField]
    private float _contentDownOffset;

    private Vector2 _contentNormalPos;

    private void Awake()
    {
        ButtonEventForwarder btn = GetComponentInChildren<ButtonEventForwarder>();
        btn.onSelect += OnSelect;
        btn.onDeselect += OnDeselect;
        btn.onPointerUp += OnClickUp;
        btn.onPointerDown += OnClickDown;
        btn.onSubmit += OnSubmit;
        _selectionOutline.color = _notSelectedColor;

        _contentNormalPos = _contentRect.anchoredPosition;
    }

    private void OnSelect()
    {
        _selectionOutline.DOColor(_isSelectedColor, 0.25f).SetEase(Ease.OutExpo);
    }

    private void OnDeselect()
    {
        _selectionOutline.DOColor(_notSelectedColor, 0.25f).SetEase(Ease.OutQuad);
    }

    private void OnEnable()
    {
        _selectionOutline.color = _notSelectedColor;
    }


    private void OnDisable()
    {
        _selectionOutline.color = _notSelectedColor;
    }

    private void OnSubmit()
    {
        ChangeVisualsToDown();
        StartCoroutine(SubmitRoutine());
    }

    private void OnClickDown()
    {
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
        _image.sprite = _downSprite;
        _contentRect.anchoredPosition = _contentNormalPos + new Vector2(0, _contentDownOffset);
    }

    private void ChangeVisualsToUp()
    {
        _image.sprite = _upSprite;
        _contentRect.anchoredPosition = _contentNormalPos;
    }
}
