using UnityEngine;

public class Collectable : MonoBehaviour
{
    private LevelManager _levelManager;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
