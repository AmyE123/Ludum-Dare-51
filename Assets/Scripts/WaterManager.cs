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

    [SerializeField]
    private float _timeUntilRise;

    [SerializeField]
    private Transform _waterTopTransform;

    [SerializeField]
    private Transform _waterFrontTransform;

    [SerializeField]
    private AudioSource _waterSound;

    [SerializeField]
    private SaveData _saveData;

    private bool _isMoving = false;

    public float TimeUntilRise => Mathf.Clamp(_timeUntilRise, 0, WATER_RISING_TIME);

    public float PercentDone => TimeUntilRise / WATER_RISING_TIME;

    public float speedMultiplier = 1;

    private bool _isLevelComplete;

    public void SetLevelComplete() => _isLevelComplete = true;

    private int _maxWaterLevel = 99;

    public void SetMaxWaterLevel(int val)
    {
        if (val == 0)
            return;

        _maxWaterLevel = val;
    }
    
    public float DisplayPercent
    {
        get
        {
            if (_isMoving)
                return 0;

            if (_waterHeight >= _maxWaterLevel)
                return 0;

            float timeBetweenRise = WATER_RISING_TIME - _waterRiseDelay;
            return Mathf.Clamp01(_timeUntilRise / timeBetweenRise);
        }
    }

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

        foreach (ParticleController particle in _splashVFX)
        {
            particle.isEmitting = false;
        }
    }

    private void Update()
    {
        float dist = _waterTopTransform.position.y - _waterFrontTransform.position.y;
        _waterFrontTransform.localScale = new Vector3(1, dist, 1);

        UpdateWaterTimer();

        foreach (ParticleController particle in _splashVFX)
        {
            particle.isEmitting = _isMoving;
        }
    }

    private void UpdateWaterTimer()
    {
        if (PauseMenu.IsGamePaused || _isLevelComplete)
            return;

        if (_waterHeight >= _maxWaterLevel)
            return;

        if (_isMoving)
            _timeUntilRise -= Time.deltaTime;
        else
            _timeUntilRise -= Time.deltaTime * speedMultiplier;


        if (_timeUntilRise <= 0)
        {
            IncrementWaterLevel();
            _timeUntilRise += WATER_RISING_TIME;
        }
    }

    private void SetNewWaterLevel()
    {
        _isMoving = true;

        _waterSound.DOFade(0.25f * _saveData.SoundVolume, 0.3f);
        _waterTransform.DOLocalMoveY(_waterHeight, _waterRiseDelay).OnComplete(() => 
        {
            _isMoving = false;
            _waterSound.DOFade(0, 0.6f);
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
