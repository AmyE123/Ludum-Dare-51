using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemPrefab;

    private List<CollectableItemUI> _children;

    int _filled;

    public void Init(int collectibleCount)
    {
        _children = new List<CollectableItemUI>();

        for (int i=0; i<collectibleCount; i++)
        {
            GameObject newObj = Instantiate(_itemPrefab, transform);
            _children.Add(newObj.GetComponent<CollectableItemUI>());
        }
    }

    public void OnCollectibleGot()
    {
        if (_filled < _children.Count)
            _children[_filled].Unlock();
            
        _filled ++;
    }
}
