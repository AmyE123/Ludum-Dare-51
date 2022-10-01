using DG.Tweening;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private const int WATER_RISING_TIME = 10;
    private const int WATER_RISING_DELAY = 5;

    [SerializeField]
    [Range(0, 10)]
    private int _waterHeight;

    [SerializeField]
    private Transform _waterTransform;

    private bool _isMoving = false;

    [SerializeField]
    private float _riseTime;

    public int WaterHeight => _waterHeight;

    private void Start()
    {
        InitializeValues();
        _waterHeight = 0;
    }

    private void InitializeValues()
    {
        _riseTime = WATER_RISING_TIME;
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
            _riseTime -= Time.deltaTime;

            if (_riseTime <= 0)
            {
                RiseWater();
                _riseTime = WATER_RISING_TIME;
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
        _waterTransform.DOLocalMoveY(_waterHeight, WATER_RISING_DELAY);
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
