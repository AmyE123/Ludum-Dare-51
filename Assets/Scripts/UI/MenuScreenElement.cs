using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuScreenElement : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private bool _doRect;
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTweenValues _rectInTween;
    [SerializeField] private RectTweenValues _rectOutTween;

    [Header("Graphic")]
    [SerializeField] private bool _doGraphic;
    [SerializeField] private Graphic _graphic;
    [SerializeField] private GraphicTweenValues _graphicInTween;
    [SerializeField] private GraphicTweenValues _graphicOutTween;

    [Header("Canvas Group")]
    [SerializeField] private bool _doGroup;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private CanvasGroupTweenValues _groupInTween;
    [SerializeField] private CanvasGroupTweenValues _groupOutTween;
    
    #if UNITY_EDITOR // This is for the custom inspector, we don't want these public outside of that
    public bool DoRect { get => _doRect; set => _doRect = value; }
    public RectTweenValues RectInTween { get => _rectInTween; set => _rectInTween = value; }
    public RectTweenValues RectOutTween { get => _rectOutTween; set => _rectOutTween = value; }

    public bool DoGraphic { get => _doGraphic; set => _doGraphic = value; }
    public GraphicTweenValues GraphicInTween { get => _graphicInTween; set => _graphicInTween = value; }
    public GraphicTweenValues GraphicOutTween { get => _graphicOutTween; set => _graphicOutTween = value; }

    public bool DoGroup { get => _doGroup; set => _doGroup = value; }
    public CanvasGroupTweenValues GroupInTween { get => _groupInTween; set => _groupInTween = value; }
    public CanvasGroupTweenValues GroupOutTween { get => _groupOutTween; set => _groupOutTween = value; }
    #endif

    void Reset()
    {
        _rect = GetComponent<RectTransform>();
        _graphic = GetComponent<Graphic>();
        _group = GetComponent<CanvasGroup>();
    }

    public void SnapToInactive()
    {
        if (_doRect && _rect != null)
        {
            if (_rectOutTween.doAnchoredPosition && _rectOutTween.doTween)
            _rect.anchoredPosition = _rectOutTween.anchoredPosition;

            if (_rectOutTween.doScale && _rectOutTween.doTween)
                _rect.localScale = _rectOutTween.localScale;
        }

        if (_doGraphic && _graphic != null)
        {
            if (_graphicOutTween.doColor && _graphicOutTween.doTween)
                _graphic.color = _graphicOutTween.color;

            if (_graphicOutTween.doAlpha && _graphicOutTween.doTween)
            {
                Color c = _graphic.color;
                _graphic.color = new Color(c.r, c.g, c.b, _graphicOutTween.alpha);
            }
        }

        if (_doGroup && _group != null)
        {
            if (_groupOutTween.doAlpha && _groupOutTween.doTween)
                _group.alpha = _groupOutTween.alpha;
        }
    }

    public void AnimateIn(float delay=0)
    {
        if (_rect != null && _doRect && _rectInTween.doTween)
            AnimateRect(_rectOutTween, _rectInTween, delay);

        if (_graphic != null && _doGraphic && _graphicInTween.doTween)
            AnimateGraphic(_graphicOutTween, _graphicInTween, delay);

        if (_group != null && _doGroup && _groupInTween.doTween)
            AnimateCanvasGroup(_groupOutTween, _groupInTween, delay);
    }

    public void AnimateOut(float delay=0)
    {
        if (_rect != null && _doRect && _rectOutTween.doTween)
            AnimateRect(_rectInTween, _rectOutTween, delay);

        if (_graphic != null && _doGraphic && _graphicOutTween.doTween)
            AnimateGraphic(_graphicInTween, _graphicOutTween, delay);

        if (_group != null && _doGroup && _groupOutTween.doTween)
            AnimateCanvasGroup(_groupInTween, _groupOutTween, delay);
    }

    private void AnimateRect(RectTweenValues fromVal, RectTweenValues toVal, float delay=0)
    {
        DOTween.Kill(_rect);
        
        // Resetting the position/scale to the fromValue
        if (fromVal.doAnchoredPosition && fromVal.doTween)
            _rect.anchoredPosition = fromVal.anchoredPosition;

        if (fromVal.doScale && fromVal.doTween)
            _rect.localScale = fromVal.localScale;

        // Starting the tweens towards the toValue
        if (toVal.doAnchoredPosition)
            _rect.DOAnchorPos(toVal.anchoredPosition, toVal.time).SetEase(toVal.ease).SetDelay(delay);

        if (toVal.doScale)
            _rect.DOScale(toVal.localScale, toVal.time).SetEase(toVal.ease).SetDelay(delay);
    }

    private void AnimateGraphic(GraphicTweenValues fromVal, GraphicTweenValues toVal, float delay=0)
    {
        DOTween.Kill(_graphic);
        
        // Resetting the color/alpha to the fromValue
        if (fromVal.doColor && fromVal.doTween)
            _graphic.color = fromVal.color;

        if (fromVal.doAlpha && fromVal.doTween)
        {
            Color c = _graphic.color;
            _graphic.color = new Color(c.r, c.g, c.b, fromVal.alpha);
        }

        // Starting the tweens towards the toValue
        if (toVal.doColor)
            _graphic.DOColor(toVal.color, toVal.time).SetEase(toVal.ease).SetDelay(delay);

        if (toVal.doAlpha)
            _graphic.DOFade(toVal.alpha, toVal.time).SetEase(toVal.ease).SetDelay(delay);
    }

    private void AnimateCanvasGroup(CanvasGroupTweenValues fromVal, CanvasGroupTweenValues toVal, float delay=0)
    {
        DOTween.Kill(_group);
        
        // Resetting the alpha to the fromValue
        if (fromVal.doAlpha && fromVal.doTween)
            _group.alpha = fromVal.alpha;

        // Starting the tween towards the toValue
        if (toVal.doAlpha)
            _group.DOFade(toVal.alpha, toVal.time).SetEase(toVal.ease).SetDelay(delay);
    }

    [System.Serializable]
    public class RectTweenValues
    {
        public bool doTween;

        public float time = 0.5f;
        public Ease ease = Ease.OutQuad;

        public bool doAnchoredPosition;
        public Vector2 anchoredPosition;

        public bool doScale;
        public Vector3 localScale;
    }

    [System.Serializable]
    public class GraphicTweenValues
    {
        public bool doTween;

        public float time = 0.5f;
        public Ease ease = Ease.OutQuad;

        public bool doColor;
        public Color color = new Color(1, 1, 1, 1);
        
        public bool doAlpha;
        [Range(0,1)]
        public float alpha;
    }

    [System.Serializable]
    public class CanvasGroupTweenValues
    {
        public bool doTween;

        public float time = 0.5f;
        public Ease ease = Ease.OutQuad;
        
        public bool doAlpha;
        [Range(0,1)]
        public float alpha;
    }
}
