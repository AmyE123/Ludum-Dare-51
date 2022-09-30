using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class ScreenManager : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float _transitionTime = 0.8f;
    [SerializeField] private float _newScreenDelay = 0.2f;
    [SerializeField] private Ease _ease = Ease.OutExpo;
    
    [Space(12)]
    [Tooltip("How far should the screen move when going to the next menu?")]
    [SerializeField] private Vector2 _transitionOffset = new Vector2(-64, 0);

    private List<MenuScreen> _menuStack = new List<MenuScreen>();

    protected void InitStack(MenuScreen rootScreen)
    {
        _menuStack = new List<MenuScreen>();
        _menuStack.Add(rootScreen);
    }

    public void GoBackOneLevel()
    {
        if (_menuStack.Count <= 1)
        {
            Debug.LogWarning("Can't go back a level, we're on the base level!");
            return;
        }

        MenuScreen topLevelMenu = _menuStack[_menuStack.Count-1];
        MenuScreen prevLevelMenu = _menuStack[_menuStack.Count-2];

        _menuStack.RemoveAt(_menuStack.Count-1);

        topLevelMenu.MoveOffScreen(-_transitionOffset, _transitionTime, _ease);
        prevLevelMenu.MoveOntoScreen(_transitionOffset, _transitionTime, _newScreenDelay, _ease);
    }

    protected void ShowScreen(MenuScreen newScreen)
    {
        if (_menuStack.Count > 0)
        {
            MenuScreen topLevelMenu = _menuStack[_menuStack.Count-1];
            topLevelMenu.MoveOffScreen(_transitionOffset, _transitionTime, _ease);
        }

        _menuStack.Add(newScreen);
        newScreen.MoveOntoScreen(-_transitionOffset, _transitionTime, _newScreenDelay, _ease);
    }
}
