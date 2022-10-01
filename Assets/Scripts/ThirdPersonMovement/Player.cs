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
    private int _killDelay = 3;

    [SerializeField]
    private float _timeUntilKill;

    [SerializeField]
    private bool _hasHitWater;

    #region TEMP
    [SerializeField]
    private GameObject _tempUI;

    [SerializeField]
    private Volume _tempPostProcessingVolume;
    [SerializeField]
    private Vignette _vg;
    [SerializeField]
    private ColorAdjustments _ca;
    [SerializeField]
    private ChromaticAberration _chrA;
    #endregion

    PersonMovement _movement;
    CameraFollow _cameraFollow;

    private void Start()
    {
        InitializeValues();
        _tempUI.SetActive(false);

        _tempPostProcessingVolume.profile.TryGet(out _vg);
        _tempPostProcessingVolume.profile.TryGet(out _ca);
        _tempPostProcessingVolume.profile.TryGet(out _chrA);
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
            
            _chrA.intensity.value += Time.deltaTime;
            _ca.saturation.value -= Time.deltaTime * 30;

            if (_vg.intensity.value <= 0.3f)
            {
                _vg.intensity.value += Time.deltaTime / 30;
            }

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
        _tempUI.SetActive(true);
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

    public void TEMP_RetryBTN()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TEMP_HomeBTN()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
