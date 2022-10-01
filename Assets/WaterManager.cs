using DG.Tweening;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private const int WATER_RISING_TIME = 10;

    [SerializeField]
    [Range(0, 10)]
    private int _waterHeight;

    [SerializeField]
    private Transform _waterTransform;

    [SerializeField]
    private int _waterRiseDelay = 5;

    private bool _isMoving = false;

    [SerializeField]
    private float _timeUntilRise;

    public float TimeUntilRise => Mathf.Clamp(_timeUntilRise, 0, WATER_RISING_TIME);
    //[SerializeField]
    //private GameObject _VFXContainer;

    [SerializeField]
    private ParticleController[] _splashVFX;

    public int WaterHeight => _waterHeight;

    public float WaterHeightExact => _waterTransform.position.y;

    private void Start()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        _timeUntilRise = WATER_RISING_TIME;
        _waterHeight = 0;
        //_VFXContainer.SetActive(false);

        foreach (ParticleController particle in _splashVFX)
        {
            particle.isEmitting = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _timeUntilRise = WATER_RISING_TIME;
            IncrementWaterLevel();
        } 

        UpdateWaterTimer();

        foreach (ParticleController particle in _splashVFX)
        {
            particle.isEmitting = _isMoving;
        }
    }

    private void UpdateWaterTimer()
    {
        if (PauseMenu.IsGamePaused)
            return;

        _timeUntilRise -= Time.deltaTime;

        if (_timeUntilRise <= 0 && RollingLog.NumberRolling == 0)
        {
            IncrementWaterLevel();
            _timeUntilRise = WATER_RISING_TIME;
        }
    }

    private void SetNewWaterLevel()
    {
        _isMoving = true;        
        _waterTransform.DOLocalMoveY(_waterHeight, _waterRiseDelay).OnComplete(() => 
        {
            _isMoving = false;
        });
    }

    public void IncrementWaterLevel()
    {
        if (RollingLog.NumberRolling != 0)
            return;

        _waterHeight++;
        SetNewWaterLevel();
    }

    public void DecrementWaterLevel()
    {
        if (RollingLog.NumberRolling != 0)
            return;
            
        _waterHeight--;
        SetNewWaterLevel();
    }
}
