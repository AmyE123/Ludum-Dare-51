using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private PauseMenu _pauseMenu;

    [SerializeField]
    private DeathMenu _deathMenu;

    void Start()
    {
        _pauseMenu.SnapClosed();
    }

    void Update()
    {
        if (_deathMenu.IsOnScreen)
            return;
            
        if (Input.GetButtonDown("Menu"))
            _pauseMenu.ToggleMenu();

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
}
