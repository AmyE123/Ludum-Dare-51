using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<Collectable> _uncollectedCollectables;

    [SerializeField]
    private List<Collectable> _collectedCollectables;

    void Start()
    {
        _uncollectedCollectables.AddRange(FindObjectsOfType<Collectable>());
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
            Debug.Log("Completed Level");
        }
    }
}
