using System.Collections;
using System.Collections.Generic;
using ThirdPersonMovement;
using UnityEngine;

public class RollingLog : Pushable
{
    private static int _numRolling = 0;

    public static int NumberRolling => _numRolling;

    protected override bool CanMoveLeftRight => _collider.bounds.extents.x < _collider.bounds.extents.z;
    protected override bool CanMoveForwardBack => _collider.bounds.extents.x > _collider.bounds.extents.z;

    protected override IEnumerator PushRoutine(Vector3 direction, PersonPushController player)
    {
        float distanceToMove = 1f;
        float t = 0;

        player.LockIntoPushing();

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + (direction * distanceToMove);

        _numRolling ++;

        bool isFirstPush = true;

        while (t < 1)
        {
            t += Time.deltaTime * _pushSpeed;
            Vector3 prevPosition = transform.position;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            Vector3 amountMoved = transform.position - prevPosition;
    
            if (isFirstPush)
                player.PushableMoved(amountMoved);
    
            yield return null;

            if (t >= 1)
            {
                if (isFirstPush)
                    player.UnlockPushing();

                if (CheckIfShouldFall())
                {

                }
                else if (CheckIfCanMove(direction))
                {
                    startPos = transform.position;
                    endPos = transform.position + (direction * distanceToMove);
                    t = 0;
                    isFirstPush = false;
                }
            }
        }

        _numRolling --;
    }
}
