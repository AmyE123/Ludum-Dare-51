using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private PauseMenu _pauseMenu;

    void Start()
    {
        _pauseMenu.SnapClosed();
    }

    void Update()
    {
        if (Input.GetButtonDown("Menu"))
            _pauseMenu.ToggleMenu();

    }
}
