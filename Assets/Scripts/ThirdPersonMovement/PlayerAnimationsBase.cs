using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class PlayerAnimationsBase : MonoBehaviour
    {
        [SerializeField] protected float _skidSpeedThreshold = 1f;
        [SerializeField] private ParticleController _skidParticles;
        [SerializeField] private ParticleController _grindParticles;
        [SerializeField] private float _animationSpeedMul = 1f;
        [SerializeField] protected Animator _anim;

        private float _moveSpeed;
        private bool _isRunning;
        private bool _isGrounded;
        private float _runAnimation;

        public virtual void DoUpdate()
        {
            _anim.speed = _animationSpeedMul;
        }

        protected void SetIsRunning(bool value)
        {
            if (_isRunning == value)
                return;

            _isRunning = value;
            OnIsRunningChanged(_isRunning);
        }

        protected void SetIsGrounded(bool value)
        {
            if (_isGrounded == value)
                return;

            _isGrounded = value;
            OnIsGroundedChanged(_isGrounded);
        }

        protected void SetRunAnimation(float value)
        {
            if (_runAnimation == value)
                return;

            _runAnimation = value;
            OnRunAnimationChanged(_runAnimation);
        }

        protected void SetMoveSpeed(float value)
        {
            if (_moveSpeed == value)
                return;

            _moveSpeed = value;
            OnMoveSpeedChanged(_moveSpeed);
        }

        protected virtual void OnIsRunningChanged(bool newValue)
        {
            _anim.SetBool("isRunning", newValue);
        }

        protected virtual void OnIsGroundedChanged(bool newValue)
        {
            _anim.SetBool("isGrounded", newValue);
        }

        protected virtual void OnMoveSpeedChanged(float newValue)
        {
            _anim.SetFloat("moveSpeed", newValue);
        }

        protected virtual void OnRunAnimationChanged(float newValue)
        {
            _anim.SetFloat("runAnimation", newValue);
        }

        public virtual void DoDie()
        {
            _anim.ResetTrigger("deathTrigger");
            _anim.SetTrigger("deathTrigger");
        }

        public virtual void DoJump()
        {
            _anim.ResetTrigger("jumpTrigger");
            _anim.SetTrigger("jumpTrigger");
        }

        public virtual void DoAirJump()
        {
            _anim.ResetTrigger("airJumpTrigger");
            _anim.SetTrigger("airJumpTrigger");
        }

        public virtual void BeginSkid(float speed)
        {
            if (speed < _skidSpeedThreshold)
                return;

            _anim.ResetTrigger("stopSkid");
            _anim.ResetTrigger("startSkid");
            _anim.SetTrigger("startSkid");
            _skidParticles.isEmitting = true;
        }

        public virtual void EndSkid()
        {
            _anim.ResetTrigger("startSkid");
            _anim.ResetTrigger("stopSkid");
            _anim.SetTrigger("stopSkid");
            _skidParticles.isEmitting = false;
        }

        public virtual void SetGrinding(bool isGrinding)
        {
            _anim.SetBool("isGrinding", isGrinding);
            _grindParticles.isEmitting = isGrinding;
        }

        public virtual void SetGrindLeft(bool isLeft)
        {
            _anim.SetFloat("grindDirection", isLeft ? 0 : 1);
        }
    }
}