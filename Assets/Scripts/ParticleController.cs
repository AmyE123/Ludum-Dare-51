using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particles;

    [SerializeField]
    private float _rateOverTime;

    [SerializeField]
    private float _rateOverDistance;

    public bool isEmitting;

    void Update()
    {
        var emit = _particles.emission;
        emit.rateOverDistance = isEmitting ? _rateOverDistance : 0;
        emit.rateOverTime = isEmitting ? _rateOverTime : 0;
    }
}
