using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class PersonGrindController : PersonSubController
    {
        [SerializeField]
        private Vector3 _middleOffset = Vector3.up;

        [SerializeField]
        private ParticleController _particles;

        [SerializeField, Range(0, 2)]
        private float _gravityMultiplier = 1f;

        [SerializeField]
        private Player _player;

        private GrindRail _activeRail;

        private int _cooldownFrames;

        private float _velocity;

        private bool _isBackwards;

        void Start()
        {
            _particles.isEmitting = false;
        }

        public void EnterRail(GrindRail rail)
        {
            if (_cooldownFrames > 0)
                return;
            
            // only start grinding if not already grinding
            if (_activeRail != null)
                return;

            // only start grinding if falling
            if (_movement.ActualVelocity.y > -0.05f)
                return;

            // If we're not actually above the rail then ignore it
            if (rail.IsPositionOnRail(transform.position) == false)
                return;
            
            _activeRail = rail;
            BeginGrind(rail);
        }

        public void ExitRail(GrindRail rail)
        {
            if (_activeRail != rail)
                return;

            _activeRail = null;
            EndGrind();
        }

        public void BeginGrind(GrindRail rail)
        {
            float velocityAlignment = 0;

            Vector3 railForward = rail.Forward;
            Quaternion railRotation = Quaternion.LookRotation(railForward, Vector3.up);
            transform.position = rail.FindClosestPoint(transform.position) + _middleOffset;

            if (_movement.ActualVelocity.magnitude > 0)
                velocityAlignment = Vector3.Dot( rail.Forward, _movement.ActualVelocity.normalized);

            _velocity = _movement.ActualVelocity.magnitude * velocityAlignment;
            _isBackwards = _velocity < 0;

            Vector3 localVector = rail.Rotation * transform.forward;
            bool isLeft = localVector.x > 0 == _isBackwards;

            _movement.TakeOver(this, true);
            _anim.SetGrinding(true);
            _anim.SetGrindLeft(isLeft);
        }

        public void EndGrind()
        {
            _jumpRequested = false;
            _movement.StopTakeOver(this, true);
            _anim.SetGrinding(false);
            _cooldownFrames = 0;
            _activeRail = null;
        }

        void FixedUpdate()
        {
            if (_cooldownFrames > 0)
                _cooldownFrames --;

            if (_activeRail != null)
                HandleGrinding();     
        }

        private void HandleGrinding()
        {
            if (_activeRail == null)
                return;

            float grav = Vector3.Dot(Vector3.down, _activeRail.Forward);
            _velocity += grav * -Physics.gravity.y * _gravityMultiplier * Time.deltaTime;

            transform.position += _activeRail.Forward * _velocity * Time.deltaTime;

            if (_activeRail.IsPositionOnRail(transform.position) == false)
            {
                _movement.ForceSetVelocity(_velocity * _activeRail.Forward);
                transform.rotation = Quaternion.LookRotation((_activeRail.Forward * _velocity).normalized, Vector3.up);
                EndGrind();
            }

            if (_jumpRequested)
            {
                Vector3 newVelocity = _velocity * _activeRail.Forward;

                transform.rotation = Quaternion.LookRotation((_activeRail.Forward * _velocity).normalized, Vector3.up);
                transform.position += new Vector3(0, 0.2f, 0);
                EndGrind();
                
                _movement.ForceSetVelocity(newVelocity);
                _movement.ForceFirstJump();
            }
        }

        public override void HandleFacingDirection()
        {
            if (_activeRail == null)
                return;

            if (_isBackwards)
                transform.rotation = Quaternion.LookRotation(-_activeRail.Forward, Vector3.up);
            else
                transform.rotation = Quaternion.LookRotation(_activeRail.Forward, Vector3.up);
        }
    }
}