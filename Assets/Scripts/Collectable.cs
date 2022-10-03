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

    [SerializeField]
    private AudioClip _collectSound;

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
            FindObjectOfType<CollectableUI>().OnCollectibleGot();
            _levelManager.Collect(this);
            UnspawnCollectable();

            if (_collectSound != null)
                SfxManager.PlaySoundRandomPitch(_collectSound, 0.8f, 1.2f);
        }
    }

    private void UnspawnCollectable()
    {
        transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}
