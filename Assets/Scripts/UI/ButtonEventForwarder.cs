using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

// Forwards events to AdvancedButton
public class ButtonEventForwarder : Button
{
    public Action onSelect;
    public Action onDeselect;
    public Action onSubmit;
    public Action onPointerDown;
    public Action onPointerUp;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onSelect?.Invoke();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        onDeselect?.Invoke();
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        onSubmit?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onPointerDown?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUp?.Invoke();
    }
}
