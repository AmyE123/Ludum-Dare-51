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
        [SerializeField] private PersonPushController _pusher;
        [SerializeField] private float _runSpeedMultiplier = 1f;
        [SerializeField] private SkinnedMeshRenderer _faceMesh;

        private bool _hasWon;

        public override void DoUpdate()
        {
            base.DoUpdate();
            SyncVars();
        }

        void Start()
        {
            SetMouthOpen(false);
        }

        void SyncVars()
        {
            if (_hasWon)
                return;
            
            float vel = _rigidBody.velocity.magnitude;

            SetMoveSpeed(Mathf.Max(vel * _runSpeedMultiplier, 0.1f));
            SetIsRunning(_playerMove.IsGrounded && vel > 0.1f);
            SetIsGrounded(_playerMove.IsGrounded);
            SetRunAnimation(_runMapping.Evaluate(_rigidBody.velocity.magnitude));
            SetPushing(_pusher.IsWarmingUp, _pusher.IsMoving);
        }

        private void SetPushing(bool pushing, bool moving)
        {
            _anim.SetBool("isPushing", pushing);
            _anim.SetBool("isPushMoving", moving);
        }

        public void DoFinishWink()
        {
            SetRightEyeOpen(true);
            SetLeftEyeOpen(false);
            SetMouthOpen(true);
        }

        public override void DoVictoryAnim()
        {
            _hasWon = true;
            SetPushing(false, false);
            _anim.ResetTrigger("winTrigger");
            _anim.SetTrigger("winTrigger");
        }

        public void SetRightEyeOpen(bool open) => _faceMesh.SetBlendShapeWeight(2, open ? 0 : 100);

        public void SetLeftEyeOpen(bool open) => _faceMesh.SetBlendShapeWeight(1, open ? 0 : 100);

        public void SetMouthOpen(bool open) => _faceMesh.SetBlendShapeWeight(0, open ? 0 : 100);
    }
}
