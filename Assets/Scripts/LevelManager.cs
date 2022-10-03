using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<Collectable> _uncollectedCollectables;

    [SerializeField]
    private List<Collectable> _collectedCollectables;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private LevelList _levels;

    [SerializeField]
    private CameraFollow _camera;

    [SerializeField]
    private GameUI _gameUI;

    [SerializeField]
    private WaterManager _water;

    [SerializeField]
    private SaveData _saveData;

    private LevelLayout _spawnedLevel;

    void Start()
    {
        SpawnLevel(_levels.CurrentLevel);
        _uncollectedCollectables.AddRange(FindObjectsOfType<Collectable>());
        _gameUI.InitCollectibles(_uncollectedCollectables.Count);
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
            OnLevelComplete();
        #endif
    }

    public void OnLevelComplete()
    {
        _player.WinLevel();
        _camera.SwapToWinMode();
        _gameUI.LevelComplete();
        _water.SetLevelComplete();
        _saveData.CompleteLevel(_levels.CurrentLevel);
    }

    private void SpawnLevel(LevelData level)
    {
        if (level == null)
        {
            Debug.LogError("Tried to spawn null level!");
            return;
        }

        if (level.LevelPrefab == null)
        {
            Debug.LogError($"Missing prefab from level: {level.name}!");
            return;
        }

        GameObject spawnedObj = Instantiate(level.LevelPrefab);
        _spawnedLevel = spawnedObj.GetComponent<LevelLayout>();
        
        _player.transform.position = _spawnedLevel.SpawnPosition;
    }

    public void Collect(Collectable collectable)
    {
        _uncollectedCollectables.Remove(collectable);
        _collectedCollectables.Add(collectable);
        CheckLevelCompletion();
    }

    public void CheckLevelCompletion()
    {
        if (_uncollectedCollectables.Count <= 0)
        {
            OnLevelComplete();
        }
    }
}
