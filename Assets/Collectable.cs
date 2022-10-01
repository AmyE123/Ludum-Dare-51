using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private LevelManager _levelManager;
    private bool _isCollected;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            _levelManager.Collect(this);
            UnspawnCollectable();
        }
    }

    private void UnspawnCollectable()
    {
        _isCollected = true;
        transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}
