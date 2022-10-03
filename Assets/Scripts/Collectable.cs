using UnityEngine;
using DG.Tweening;

public class Collectable : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private float _spinSpeed;

    [SerializeField]
    private Transform _childTransform;

    [SerializeField]
    private float _bobAmount;

    [SerializeField]
    private float _bobSpeed;

    private float _bobPhase;

    private LevelManager _levelManager;

    private void Update()
    {
        _bobPhase += Time.deltaTime * _bobSpeed;
        
        transform.Rotate(new Vector3(0, _spinSpeed * Time.deltaTime, 0));
        _childTransform.localPosition = new Vector3(0, Mathf.Sin(_bobPhase) * _bobAmount, 0);
    }

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
        transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}
