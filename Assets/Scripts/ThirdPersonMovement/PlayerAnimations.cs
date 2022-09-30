using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class PlayerAnimations : PlayerAnimationsBase
    {
        public bool IsRunning = false;

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private PersonMovement _playerMove;
        [SerializeField] private AnimationCurve _runMapping;
        [SerializeField] private float _runSpeedMultiplier = 1f;

        public override void DoUpdate()
        {
            base.DoUpdate();
            SyncVars();
        }

        void SyncVars()
        {
            float vel = _rigidBody.velocity.magnitude;

            SetMoveSpeed(Mathf.Max(vel * _runSpeedMultiplier, 0.1f));
            SetIsRunning(_playerMove.IsGrounded && vel > 0.1f);
            SetIsGrounded(_playerMove.IsGrounded);
            SetRunAnimation(_runMapping.Evaluate(_rigidBody.velocity.magnitude));
        }
    }
}
