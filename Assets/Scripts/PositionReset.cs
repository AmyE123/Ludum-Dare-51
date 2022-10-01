using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    private const string KillTag = "KillPlane";

    [SerializeField]
    private WaterManager _waterManager;

    [SerializeField]
    private Vector3[] _resetPositions;

    [SerializeField]
    private float _zKill;
    
    [SerializeField]
    private bool _setOnStart;

    private bool _isHit;

    void Start()
    {
        if (_setOnStart)
            _resetPositions[0] = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == KillTag)
        {
            _isHit = true;
        }
    }

    void Update()
    {
        if (transform.position.y < _zKill || _isHit)
        {
            ResetPosition();
            _isHit = false;
        }
    }

    void ResetPosition()
    {
        transform.position = _resetPositions[_waterManager.WaterHeight];
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
