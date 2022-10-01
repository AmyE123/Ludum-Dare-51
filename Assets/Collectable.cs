using UnityEngine;

public class Collectable : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private LevelManager _levelManager;

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
        gameObject.SetActive(false);
    }
}
