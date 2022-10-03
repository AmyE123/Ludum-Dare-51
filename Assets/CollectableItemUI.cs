using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectableItemUI : MonoBehaviour
{
    [SerializeField]
    RectTransform _innerItem;

    [SerializeField]
    private float _appearTime = 0.5f;

    bool _isUnlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        _innerItem.localScale = Vector3.zero;    
    }

    public void Unlock()
    {
        if (_isUnlocked)
            return;

        _isUnlocked = true;
        _innerItem.DOScale(1, _appearTime).SetEase(Ease.OutBack);
    }  
}
