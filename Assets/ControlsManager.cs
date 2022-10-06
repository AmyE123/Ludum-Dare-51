using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum CurrentControllerType { Unknown, PlayStation, Xbox, Nintendo, KeyboardMouse };

public class ControlsManager : MonoBehaviour
{
    private CurrentControllerType _currentController = CurrentControllerType.KeyboardMouse;
    private HydroRobiaInput _input;

    private void Start()
    {
        _input = new HydroRobiaInput();

        _input.Player.Enable();
        _input.Player.Jump.performed += InputSystemEvent;
        _input.Player.Menu.performed += InputSystemEvent;
        _input.Player.Move.performed += InputSystemEvent;
        _input.Player.Fastforward.performed += InputSystemEvent;

        _input.UI.Enable();
        _input.UI.Navigate.performed += InputSystemEvent;
        _input.UI.Submit.performed += InputSystemEvent;
    }

    private void OnControlTypeChanged(CurrentControllerType newType)
    {
        _currentController = newType;
        Debug.Log($"Changed to: {newType}");
    }

    private void InputSystemEvent(InputAction.CallbackContext context)
    {
        CurrentControllerType conType = GetInputType(context.control.device);

        if (conType !=  _currentController)
            OnControlTypeChanged(conType);
    }

    private CurrentControllerType GetInputType(InputDevice dev)
    {
        if (dev.description.deviceClass == "Keyboard" || dev.description.deviceClass == "Mouse")
            return CurrentControllerType.KeyboardMouse;

        if (dev.description.manufacturer == "Sony Interactive Entertainment")
            return CurrentControllerType.PlayStation;

        if (dev.description.manufacturer == "Nintendo")
            return CurrentControllerType.Nintendo;
        
        if (dev.description.interfaceName == "XInput")
            return CurrentControllerType.Xbox;

        return CurrentControllerType.Unknown;
    }
}
