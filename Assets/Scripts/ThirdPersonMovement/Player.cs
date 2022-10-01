using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonMovement;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    PersonMovement _movement;
    CameraFollow _cameraFollow;

    void Update()
    {
        HandlePlayerInput();
    }

    void HandlePlayerInput()
    {
        if (_movement == null)
            _movement = GetComponent<PersonMovement>();
            
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();

        Vector2 playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1);

        Vector3 xComponent = playerInput.x * _cameraFollow.CameraRight;
        Vector3 yComponent = playerInput.y * _cameraFollow.CameraForward;

        _movement.ControlsInput(xComponent + yComponent);
        _movement.SetJumpRequested(Input.GetButtonDown("Jump"));
    }
}
