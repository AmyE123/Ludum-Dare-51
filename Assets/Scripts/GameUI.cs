using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private PauseMenu _pauseMenu;

    [SerializeField]
    private DeathMenu _deathMenu;

    [SerializeField]
    private WinMenu _winMenu;

    [SerializeField]
    private CollectableUI _collectibleUI;

    [SerializeField]
    private MenuScreenElement[] _gameElements;

    private HydroRobiaInput _input;

    public void InitCollectibles(int count)
    {
        _collectibleUI.Init(count);
    }

    void Start()
    {        
        _input = new HydroRobiaInput();
        _input.Player.Enable();
        _pauseMenu.SnapClosed();
        _winMenu.SnapClosed();
    }

    void Update()
    {
        if (_deathMenu.IsOnScreen || _winMenu.IsOnScreen)
            return;
            
        if (_input.Player.Menu.WasPressedThisFrame())
            _pauseMenu.ToggleMenu();
    }

    public void LevelComplete()
    {
        foreach (MenuScreenElement element in _gameElements)
        {
            element.AnimateOut();
        }
        _pauseMenu.HidePauseMenu();
        StartCoroutine(ShowWinMenu(1f));
    }

    public void PlayerDied()
    {
        foreach (MenuScreenElement element in _gameElements)
        {
            element.AnimateOut();
        }
        StartCoroutine(ShowDeathMenu(1f));
    }

    private IEnumerator ShowDeathMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pauseMenu.HidePauseMenu();
        _deathMenu.ShowDeathMenu();
    }

    private IEnumerator ShowWinMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        _pauseMenu.HidePauseMenu();
        _winMenu.ShowWinMenu();
    }
}
