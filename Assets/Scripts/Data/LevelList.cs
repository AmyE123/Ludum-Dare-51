using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelList : ScriptableObject
{
    [SerializeField]
    private LevelData[] _levels;

    [SerializeField]
    private int _currentLevelIdx;

    public IEnumerable<LevelData> Levels => _levels;

    public LevelData CurrentLevel => _currentLevelIdx < _levels.Length ? _levels[_currentLevelIdx] : null;

    public void ChooseLevel(LevelData level)
    {
        for (int i=0; i<_levels.Length; i++)
        {
            if (level == _levels[i])
                _currentLevelIdx = i;
        }
    }
}
