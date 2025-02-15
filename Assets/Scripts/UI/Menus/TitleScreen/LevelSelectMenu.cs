using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectMenu : MenuScreen
{
    [SerializeField]
    private LevelList _levels;

    [SerializeField]
    private GameObject _levelPrefab;

    [SerializeField]
    private SaveData _playerProgress;

    [SerializeField]
    private RectTransform _levelParent;

    void Start()
    {
        foreach (LevelData level in _levels.Levels)
        {
            bool hasCompleted = _playerProgress.HasCompletedLevel(level);

            GameObject newObj = Instantiate(_levelPrefab, _levelParent);
            newObj.GetComponent<LevelSelectCell>().Init(level, this, hasCompleted);

            if (_defaultSelectable == null)
                _defaultSelectable = newObj.GetComponentInChildren<Button>();
        }
    }

    public void ChooseLevel(LevelData level)
    {
        _levels.ChooseLevel(level);
        TransitionManager.LoadScene("GameScene");
    }
}
