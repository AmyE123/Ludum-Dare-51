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

    public float WaterHeightExact => _waterTransform.position.y;

    private void Start()
    {
        InitializeValues();
    }

    private void InitializeValues()
    {
        _timeUntilRise = WATER_RISING_TIME;
        _waterHeight = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _timeUntilRise = WATER_RISING_TIME;
            IncrementWaterLevel();
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _timeUntilRise = WATER_RISING_TIME;
            DecrementWaterLevel();
        }   

        UpdateWaterTimer();
    }

    private void UpdateWaterTimer()
    {
        _timeUntilRise -= Time.deltaTime;

        if (_timeUntilRise <= 0)
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
        _waterHeight++;
        SetNewWaterLevel();
    }

    public void DecrementWaterLevel()
    {
        _waterHeight--;
        SetNewWaterLevel();
    }
}
