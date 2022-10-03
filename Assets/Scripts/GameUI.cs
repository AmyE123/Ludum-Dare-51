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

    public void InitCollectibles(int count)
    {
        _collectibleUI.Init(count);
    }

    void Start()
    {
        _pauseMenu.SnapClosed();
        _winMenu.SnapClosed();
    }

    void Update()
    {
        if (_deathMenu.IsOnScreen || _winMenu.IsOnScreen)
            return;
            
        if (Input.GetButtonDown("Menu"))
            _pauseMenu.ToggleMenu();

    }

    public void LevelComplete()
    {
        _pauseMenu.HidePauseMenu();
        StartCoroutine(ShowWinMenu(1f));
    }

    public void PlayerDied()
    {
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
