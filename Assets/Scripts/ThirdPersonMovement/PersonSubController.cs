using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public abstract class PersonSubController : MonoBehaviour
    {
        protected PersonMovement _movement;
        protected PlayerAnimationsBase _anim;
        protected bool _jumpRequested;

        public void InitPerson(PersonMovement movement, PlayerAnimationsBase anim)
        {
            _movement = movement;
            _anim = anim;
        }

        public void SetJumpRequested(bool isRequested)
        {
            _jumpRequested |= isRequested;
        }

        public virtual void HandleFacingDirection()
        {
            
        }
    }
}