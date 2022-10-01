using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField]
    private WaterManager _waterManager;

    [SerializeField]
    private Vector3[] _resetPositions;

    [SerializeField]
    private float _zKill;
    
    [SerializeField]
    private bool _setOnStart;

    void Start()
    {
        if (_setOnStart)
            _resetPositions[0] = transform.position;
    }

    void Update()
    {
        if (transform.position.y < _zKill)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        transform.position = _resetPositions[_waterManager.WaterHeight];
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
