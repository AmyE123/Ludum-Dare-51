using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum CurrentControllerType { Unknown, PlayStation, Xbox, Nintendo, KeyboardMouse };

public class ControlsManager : MonoBehaviour
{
    private static CurrentControllerType _currentController = CurrentControllerType.KeyboardMouse;
    private HydroRobiaInput _input;

    public CurrentControllerType CurrentControllerType => _currentController;

    private void Start()
    {
        _input = new HydroRobiaInput();

        _input.Player.Enable();
        _input.Player.Jump.performed += InputSystemEvent;
        _input.Player.Menu.performed += InputSystemEvent;
        _input.Player.Move.performed += JoystickEvent;
        _input.Player.Fastforward.performed += InputSystemEvent;

        _input.UI.Enable();
        // _input.UI.Navigate.performed += JoystickEvent; (this causes glitches)
        _input.UI.Submit.performed += InputSystemEvent;

        OnControlTypeChanged(_currentController);
    }

    private void OnControlTypeChanged(CurrentControllerType newType)
    {
        _currentController = newType;

        foreach (var visuals in FindObjectsOfType<DynamicControllerVisuals>(true))
        {
            visuals.ControllerChanged(newType);
        }
    }

    private void JoystickEvent(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().magnitude > 0.1f)
            InputSystemEvent(context);
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
