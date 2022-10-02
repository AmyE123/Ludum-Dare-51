using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonMovement;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using Vignette = UnityEngine.Rendering.Universal.Vignette;
using ChromaticAberration = UnityEngine.Rendering.Universal.ChromaticAberration;

[RequireComponent(typeof(PersonMovement))]
public class Player : MonoBehaviour
{
    private const string KillTag = "KillPlane";

    [SerializeField]
    private int _drownTimeRequirement = 3;

    [SerializeField]
    private float _currentDrownTime;

    [SerializeField]
    private float _healthRestoreSpeed = 2;

    [SerializeField]
    private WaterManager _water;

    [SerializeField]
    private CameraPostProcessing _cameraPostProcessing;

    [SerializeField]
    private GameObject _explodePrefab;

    private bool _isDead;

    PersonMovement _movement;
    CameraFollow _cameraFollow;

    private void Start()
    {
        InitializeValues();
    }

    private void Update()
    {
        if (_isDead)
            return;

        HandlePlayerInput();
        UpdatePlayerHealth();
    }

    private void InitializeValues()
    {
        _currentDrownTime = 0;
    }

    private void UpdatePlayerHealth()
    {
        float playerBottom = transform.position.y - 0.5f;
        float waterHeight = _water.WaterHeightExact;

        float underwaterPercent = Mathf.Clamp01(waterHeight - playerBottom);
        Debug.Log($"Underwater percent is {underwaterPercent}");

        if (underwaterPercent > 0)
            _currentDrownTime += Time.deltaTime * underwaterPercent;
        else
            _currentDrownTime -= _healthRestoreSpeed * Time.deltaTime;

        _currentDrownTime = Mathf.Clamp(_currentDrownTime, 0, _drownTimeRequirement);
        float drownPercent = _currentDrownTime / _drownTimeRequirement;

        _cameraPostProcessing.SetDeadPercent(drownPercent);

        if (_currentDrownTime >= _drownTimeRequirement)
        {               
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        _isDead = true;
        gameObject.SetActive(false);
        GameObject newObj = Instantiate(_explodePrefab, transform.position, transform.rotation);
        Destroy(newObj, 10);
    }

    void HandlePlayerInput()
    {
        if (_isDead)
            return;

        if (_movement == null)
            _movement = GetComponent<PersonMovement>();
            
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();

        if (PauseMenu.IsGamePaused)
            return;

        Vector2 playerInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerInput = Vector2.ClampMagnitude(playerInput, 1);

        Vector3 xComponent = playerInput.x * _cameraFollow.CameraRight;
        Vector3 yComponent = playerInput.y * _cameraFollow.CameraForward;

        _movement.ControlsInput(xComponent + yComponent);
        _movement.SetJumpRequested(Input.GetButtonDown("Jump"));
    }

    public void TEMP_RetryBTN()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TEMP_HomeBTN()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
