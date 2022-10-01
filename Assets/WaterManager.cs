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

    public int WaterHeight => _waterHeight;

    private void Start()
    {
        InitializeValues();
        _waterHeight = 0;
    }

    private void InitializeValues()
    {
        _timeUntilRise = WATER_RISING_TIME;
    }

    private void Update()
    {
        UpdateWaterTimer();
        IncrementWaterLevel();
    }

    private void UpdateWaterTimer()
    {
        if (_isMoving)
        {
            _timeUntilRise -= Time.deltaTime;

            if (_timeUntilRise <= 0)
            {
                RiseWater();
                _timeUntilRise = WATER_RISING_TIME;
                _isMoving = false;
            }
        }
    }

    private void SetNewWaterLevel()
    {
        _isMoving = true;
    }

    private void RiseWater()
    {
        _waterTransform.DOLocalMoveY(_waterHeight, _waterRiseDelay);
    }

    public void IncrementWaterLevel()
    {
        if (!_isMoving)
        {
            _waterHeight++;
            SetNewWaterLevel();
        }
    }

    public void DecrementWaterLevel()
    {
        if (!_isMoving)
        {
            _waterHeight--;
            SetNewWaterLevel();
        }
    }
}
