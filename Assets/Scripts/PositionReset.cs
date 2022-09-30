using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    [SerializeField]
    private Vector3 _resetPosition;

    [SerializeField]
    private float _zKill;
    
    [SerializeField]
    private bool _setOnStart;

    void Start()
    {
        if (_setOnStart)
            _resetPosition = transform.position;
    }

    void Update()
    {
        if (transform.position.y < _zKill)
        {
            transform.position = _resetPosition; 
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }       
    }
}
