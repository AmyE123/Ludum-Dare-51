using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdPersonMovement.MovementInfo;

namespace ThirdPersonMovement
{
    public class PersonMovement : MonoBehaviour
    {
        [SerializeField] JumpInfo _jump;
        [SerializeField] GroundInfo _ground;
        [SerializeField] MoveInfo _move;

        [Space(8)]
        [SerializeField] Collider _collider;
        [SerializeField] PlayerAnimations _localAnims;

        public PlayerAnimationsBase Animator => _localAnims;

        [SerializeField] private List<PersonSubController> _subControllers;
        [SerializeField] private PersonSubController _activeSubController;
        
        Rigidbody _rb;
        bool _isSkidding;

        public float JumpSpeed => _jump.Speed;

        public bool IsGrounded => _ground.groundContactCount > 0;

        public Vector3 DesiredVelocity => _move.desiredVelocity;

        public Vector3 VelocityDirection => _rb.velocity.magnitude > 0.01f ? _rb.velocity.normalized : transform.forward;

        public Vector3 ActualVelocity => _rb.velocity;

        public void Start()
        {
            foreach (PersonSubController sub in _subControllers)
                sub.InitPerson(this, Animator);
        }

        public void DisableRigidbody()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public void CancelVelocity()
        {
            _move.desiredVelocity = Vector3.zero;
            _rb.velocity = Vector3.zero;
        }

        public void TakeOver(PersonSubController sub, bool becomeGhost)
        {
            _activeSubController = sub;

            if (becomeGhost)
            {
                _rb.isKinematic = true;
                _collider.isTrigger = true;
            }

            SetHasJump(true);
        }

        public void StopTakeOver(PersonSubController sub, bool becomeRigid)
        {
            _activeSubController = null;

            if (becomeRigid)
            {
                _rb.isKinematic = false;
                _collider.isTrigger = false;
            }

            SetHasJump(true);
        }

        public void ForceSetVelocity(Vector3 vel) => _rb.velocity = vel;

        public void MoveVelocityTowards(Vector3 targetVel, float maxPerFrame)
        {
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, targetVel, maxPerFrame);
        }

        public void ApplyForce(Vector3 force)
        {
            _rb.AddForce(force);
        }

        public void ControlsInput(Vector3 direction)
        {
            if (_activeSubController != null)
                return;

            SetDesiredDirection(direction);
        }

        public void SetDesiredDirection(Vector3 direction)
        {
            _move.desiredVelocity = direction * _move.tempMaxSpeed;

            direction.y = 0;
        }

        public void SetVelocity(Vector3 velocity, bool isDirection)
        {
            _move.desiredVelocity = velocity;
            _rb.velocity = velocity;

            Vector3 direction = velocity.normalized;
            direction.y = 0;
        
            if (isDirection && direction.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        public void SetJumpRequested(bool isRequested)
        {
            if (_activeSubController != null)
            {
                _activeSubController.SetJumpRequested(isRequested);
                return;
            }

            _jump.isRequested |= isRequested;
        }

        void OnValidate () 
        {
            _ground.minGroundDotProduct = Mathf.Cos(_ground.maxSlopeAngle * Mathf.Deg2Rad);
        }

        // Start is called before the first frame update
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            OnValidate();
        }

        void ClearState () 
        {
            _ground.groundContactCount = 0;
            _ground.contactNormal = Vector3.zero;
            _ground.wallContactCount = 0;
            _ground.wallNormal = Vector3.zero;
        }

        void HandleFacingDirection()
        {
            if (_activeSubController != null)
            {
                _activeSubController.HandleFacingDirection();
                return;
            }

            if (IsGrounded && _move.velocity.magnitude > 0.1f)
            {
                Vector3 desiredDirection = _move.velocity;
                desiredDirection.y = 0;
                desiredDirection.Normalize();

                if (desiredDirection.magnitude > 0.1f)
                {
                    transform.rotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                }
            }
            else if (_move.desiredVelocity.magnitude > 0.1f)
            {
                Vector3 desiredDirection = _move.desiredVelocity;
                desiredDirection.y = 0;
                desiredDirection.Normalize();

                if (desiredDirection.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 19);
                }
            }
            
        }

        void Update()
        {
            Animator.DoUpdate();
        }

        void FixedUpdate()
        {
            UpdateState();
            AdjustVelocity();
            HandleFacingDirection();

            if (_jump.isRequested) 
                Jump();

            _rb.velocity = _move.velocity;
            ClearState();
        }

        public void ForceFirstJump()
        {
            _jump.stepsSinceLastJump = 0;
            Animator.DoJump();
            SetHasJump(true);
            _rb.velocity += _jump.Speed * Vector3.up;
        }


        void Jump()
        {
            _jump.isRequested = false;

            float jumpSpeed = _jump.Speed;

            if (IsGrounded)
            {
                float alignedSpeed = Vector3.Dot(_move.velocity, _ground.contactNormal);
                if (alignedSpeed > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
                }

                _move.velocity += _ground.contactNormal  * jumpSpeed;
                SetHasJump(true);
                Animator.DoJump();
            }
            else if (false && _jump.hasExtraJump)
            {
                float boostAlignment = Vector3.Dot(ActualVelocity.normalized, transform.forward);
                boostAlignment = Mathf.Clamp01(boostAlignment);

                _move.velocity *= boostAlignment;

                _jump.hasExtraJump = false;
                _move.velocity += transform.forward * _jump.extraJumpSpeed;
                _move.velocity += Vector3.up * _jump.extraJumpLift;
                Animator.DoAirJump();
            }

            _jump.stepsSinceLastJump = 0;
        }

        void SetHasJump(bool hasit)
        {
            _jump.hasExtraJump = false;
        }

        Vector3 ProjectOnContactPlane (Vector3 vector) 
        {
            return vector - _ground.contactNormal * Vector3.Dot(vector, _ground.contactNormal);
        }

        void AdjustVelocity () 
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);

            float currentX = Vector3.Dot(_move.velocity, xAxis);
            float currentZ = Vector3.Dot(_move.velocity, zAxis);

            float acceleration = IsGrounded ? _move.maxAcceleration : _move.maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

            Vector2 currentVel = new Vector2(currentX, currentZ);
            Vector2 desiredVel = new Vector2(_move.desiredVelocity.x, _move.desiredVelocity.z);

            // Skidding
            bool isSkidding = false;

            if (IsGrounded && currentVel.magnitude > 0 && desiredVel.magnitude > 0)
            {
                isSkidding = Vector3.Dot(currentVel.normalized, desiredVel.normalized) < -0.2f;

                if (isSkidding)
                {
                    maxSpeedChange = _move.maxSkidDeceleration * Time.deltaTime;
                }
            }

            if (isSkidding == true && _isSkidding == false)
                OnBeginSkid(currentVel.magnitude);
            
            if (isSkidding == false && _isSkidding == true)
                OnFinishSkid();

            // Applying the movement
            Vector2 newVel = Vector2.MoveTowards(currentVel, desiredVel, maxSpeedChange);
            _move.velocity += xAxis * (newVel.x - currentX) + zAxis * (newVel.y - currentZ);
        }

        void UpdateState () 
        {
            _ground.stepsSinceLastGrounded += 1;
            _jump.stepsSinceLastJump += 1;
            _move.velocity = _rb.velocity;

            if (IsGrounded || SnapToGround() || CheckSteepContacts()) 
            {
                _ground.stepsSinceLastGrounded = 0;

                _move.tempMaxSpeed = Mathf.Max(_rb.velocity.magnitude, _move.maxSpeed);
    
                if (_jump.stepsSinceLastJump > 2)
                    SetHasJump(false);

                if (_ground.groundContactCount > 1) {
                    _ground.contactNormal.Normalize();
                }
            }
            else 
            {
                _ground.contactNormal = Vector3.up;
            }
        }

        bool SnapToGround () {
            if (_ground.stepsSinceLastGrounded > 1 || _jump.stepsSinceLastJump <= 2) {
                return false;
            }
            float speed = _move.velocity.magnitude;
            if (speed > _ground.maxSnapSpeed) {
                return false;
            }

            if (!Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit, _ground.probeDistance)) {
                return false;
            }

            if (hit.normal.y < _ground.minGroundDotProduct) {
                return false;
            }

            _ground.contactNormal = hit.normal;
            float dot = Vector3.Dot(_move.velocity, hit.normal);
            if (dot > 0f) {
                _move.velocity = (_move.velocity - hit.normal * dot).normalized * speed;
            }
            return true;
        }

        void OnCollisionStay (Collision collision) => EvaluateCollision(collision);

        void EvaluateCollision (Collision collision) 
        {
            for (int i = 0; i < collision.contactCount; i++) 
            {
                if (collision.gameObject.tag == "Rail")
                    continue;

                Vector3 normal = collision.GetContact(i).normal;

                if (normal.y >= _ground.minGroundDotProduct) 
                {
                    _ground.groundContactCount += 1;
                    _ground.contactNormal += normal;
                }
                else if (normal.y > -0.01f && normal.y < 0.05f) 
                {
                    _ground.wallContactCount += 1;
                    _ground.wallNormal += normal;
                }
            }
        }

        bool CheckSteepContacts () 
        {
            if (_ground.wallContactCount > 1) {
                _ground.wallNormal.Normalize();
                if (_ground.wallNormal.y >=_ground.minGroundDotProduct)
                {
                    _ground.groundContactCount = 1;
                    _ground.contactNormal = _ground.wallNormal;
                    return true;
                }
            }
            return false;
        }

        void OnBeginSkid(float speed)
        {
            _isSkidding = true;
            Animator.BeginSkid(speed);
        }

        void OnFinishSkid()
        {
            _isSkidding = false;
            Animator.EndSkid();
        }
    }
}