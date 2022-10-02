using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    [DefaultExecutionOrder(-999)]
    public class PersonPushController : PersonSubController
    {
        [SerializeField]
        private PersonMovement _player;

        [SerializeField]
        private float _warmUpTime = 0.5f;

        [SerializeField]
        private LayerMask _pushableLayer;

        private Pushable _activePushable;
        private Vector3 _pushDirection;
        private float _pushTime;
        private bool _isLockedIn;

        public bool IsMoving => _pushTime >= _warmUpTime;
        
        public bool IsWarmingUp => _pushTime > 0;

        void Start()
        {

        }

        public void StartPushing(Pushable obj)
        {
            _activePushable = obj;
            _pushTime = 0;
        }

        public void StopPushing()
        {
            _activePushable = null;
            _pushTime = 0;
        }

        void FixedUpdate()
        {
            if (_isLockedIn)
                return;

            if (_activePushable == null)
                DetectPushables();

            if (_activePushable == null)
                return;

            Vector3 pushDir = GetPushDirection();
            
            if (Vector3.Dot(pushDir, _pushDirection) < 0.6f)
            {
                _activePushable = null;
                _pushTime = 0;
                _player.StopTakeOver(this, false);
                return;
            }

            _pushTime += Time.deltaTime;

            if (_pushTime < _warmUpTime)
                return;

            HandlePushing();
        }

        private Vector3 GetPushDirection()
        {
            if (_player.IsGrounded == false)
                return Vector3.zero;
            
            Vector2 dirPressed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            dirPressed = Vector3.ClampMagnitude(dirPressed, 1);

            if (dirPressed.magnitude < 0.5f)
                return Vector3.zero;

            return new Vector3(dirPressed.x, 0, dirPressed.y);
        }

        private void DetectPushables()
        {
            Vector3 direction = GetPushDirection();

            if (direction.magnitude < 0.1f)
                return;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, _pushableLayer))
            {
                _activePushable = hit.transform.gameObject.GetComponent<Pushable>();

                if (_activePushable == null)
                    return;

                if (_activePushable.IsFalling || _activePushable.IsInWater)
                {
                    _activePushable = null;
                    return;
                }


                Vector3 dirToPush = -hit.normal;
                dirToPush.y = 0;
                dirToPush.Normalize();
                _player.TakeOver(this, false);
                _movement.CancelVelocity();

                _pushDirection = dirToPush;
            }
        }

        private void HandlePushing()
        {
            if (_activePushable == null)
                return;

            if (_isLockedIn)
                return;

            if (_movement.IsGrounded == false)
                return;

            _activePushable.Push(_pushDirection, this);
        }

        public void LockIntoPushing()
        {
            _isLockedIn = true;
            _player.TakeOver(this, false);
            _player.CancelVelocity();
        }

        public void CancelPushing()
        {
            _activePushable = null;
            _pushDirection = Vector3.zero;
            _pushTime = 0;
            _player.StopTakeOver(this, false);
            _isLockedIn = false;
        }

        public void UnlockPushing()
        {
            _isLockedIn = false;
            Vector3 direction = GetPushDirection();

            if (_activePushable.CheckIfShouldFall())
            {
                CancelPushing();
                return;
            }

            bool isStillPushing = false;

            if (direction.magnitude > 0.1f && _pushDirection.magnitude > 0.1f)
            {
                float alignment = Vector3.Dot(direction.normalized, _pushDirection.normalized);
                isStillPushing = alignment > 0.6f;
            }

            if (isStillPushing == false)
            {
                CancelPushing();
            }
            else
            {
                HandlePushing();
            }
        }

        public void PushableMoved(Vector3 amount)
        {
            transform.position += amount;
            _movement.SetDesiredDirection(Vector3.zero);
        }

        public override void HandleFacingDirection()
        {
            if (_pushDirection.magnitude < 0.1)
                return;

            Quaternion targetRot = Quaternion.LookRotation(_pushDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 5 * Time.deltaTime);
        }
    }
}