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

    [SerializeField]
    private Transform _playerMesh;

    [SerializeField]
    private float _deathVibrateAmount = 1;

    [SerializeField]
    private AudioClip _deathSound;

    private bool _isDead;
    private bool _isFinished;

    PersonMovement _movement;
    CameraFollow _cameraFollow;
    Vector3 _playerMeshRestPos;

    private void Start()
    {
        InitializeValues();
        _playerMeshRestPos = _playerMesh.localPosition;
    }

    private void Update()
    {
        if (_isFinished)
        {
            HandleFinished();
            return;
        }

        if (_isDead)
            return;

        HandlePlayerInput();
        UpdatePlayerHealth();
    }

    private void HandleFinished()
    {
        if (_currentDrownTime > 0)
        {
            _playerMesh.localPosition = _playerMeshRestPos;
            _currentDrownTime -= Time.deltaTime * _healthRestoreSpeed;
            _cameraPostProcessing.SetDeadPercent(_currentDrownTime / _drownTimeRequirement);
        }

        _movement.CancelSidewaysVelocity();
        Quaternion tgtDir = Quaternion.LookRotation(new Vector3(-0.4f, 0, -1), Vector3.up);
        transform.rotation = tgtDir;
    }

    private void InitializeValues()
    {
        _currentDrownTime = 0;
    }

    public void WinLevel()
    {
        _isFinished = true;
        _movement.Animator.DoVictoryAnim();
    }

    private void UpdatePlayerHealth()
    {
        float playerBottom = transform.position.y - 0.5f;
        float waterHeight = _water.WaterHeightExact;

        float underwaterPercent = Mathf.Clamp01(waterHeight - playerBottom);

        if (underwaterPercent > 0)
            _currentDrownTime += Time.deltaTime * underwaterPercent;
        else
            _currentDrownTime -= _healthRestoreSpeed * Time.deltaTime;

        _currentDrownTime = Mathf.Clamp(_currentDrownTime, 0, _drownTimeRequirement);
        float drownPercent = _currentDrownTime / _drownTimeRequirement;

        _cameraPostProcessing.SetDeadPercent(drownPercent);

        if (_currentDrownTime == 0)
            _playerMesh.localPosition = _playerMeshRestPos;
        else
            _playerMesh.localPosition = _playerMeshRestPos + (Random.insideUnitSphere * _deathVibrateAmount * drownPercent);

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

        FindObjectOfType<GameUI>().PlayerDied();
        
        if (_deathSound != null)
            SfxManager.PlaySound(_deathSound);
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
}
