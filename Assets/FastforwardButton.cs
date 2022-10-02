using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FastforwardButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private WaterManager _water;

    [SerializeField]
    private Image _img;

    [SerializeField]
    private Color _pressedCol;
    
    [SerializeField]
    private Color _notPressedCol;

    [SerializeField, Range(1, 20)]
    private float _speed = 8;

    [SerializeField]
    private bool _isDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDown = false;
    }


    void Update()
    {
        bool btnPressed = _isDown || Input.GetButton("Fastforward");

        _img.color = btnPressed ? _pressedCol : _notPressedCol;

        float newSpeed = btnPressed ? _speed : 1;
        _water.speedMultiplier = Mathf.Lerp(_water.speedMultiplier, newSpeed, 8 * Time.deltaTime);
    }
}
