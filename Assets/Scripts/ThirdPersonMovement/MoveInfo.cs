using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement.MovementInfo
{
    [System.Serializable]
    public class MoveInfo
    {
        [Range(0f, 100f)] public float maxAcceleration = 10f;
        [Range(0f, 100f)] public float maxSkidDeceleration = 10f;
        [Range(0f, 100f)] public float maxAirAcceleration = 1f;
        [Range(0f, 100f)] public float maxAirDecceleration = 1f;
        [Range(0f, 100f)] public float maxSpeed = 10f;

        public float tempMaxSpeed;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Vector3 desiredVelocity;
    }
}