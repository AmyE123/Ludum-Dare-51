using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonMovement;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    private const string KillTag = "KillPlane";

    [SerializeField]
    private int _killDelay = 3;

    [SerializeField]
    private float _timeUntilKill;

    [SerializeField]
    private bool _hasHitWater;

    PersonMovement _movement;
    CameraFollow _cameraFollow;

    private void Start()
    {
        InitializeValues();
    }

    private void Update()
    {
        HandlePlayerInput();
        UpdatePlayerHealth();
    }

    private void InitializeValues()
    {
        _timeUntilKill = _killDelay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == KillTag)
        {
            _hasHitWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KillTag)
        {
            _hasHitWater = false;
        }
    }

    private void UpdatePlayerHealth()
    {
        if (_hasHitWater)
        {
            _timeUntilKill -= Time.deltaTime;
            if (_timeUntilKill <= 0)
            {               
                KillPlayer();
                _timeUntilKill = _killDelay;
                _hasHitWater = false;
            }
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Die");
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
