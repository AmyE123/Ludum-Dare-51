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

    [SerializeField]
    private Transform _logTransform;

    public override void Push(Vector3 direction, PersonPushController player)
    {
        if (CheckIfCanMove(direction) == false)
        {
            player.CancelPushing();
            return;
        }

        List<Pushable> children = CheckPushablesOnTop();

        if (children.Count > 0)
        {
            player.CancelPushing();
            return;
        }

        StartCoroutine(PushRoutine(direction, player));
    }

    protected override IEnumerator PushRoutine(Vector3 direction, PersonPushController player, List<Pushable> children=null)
    {
        float distanceToMove = 1f;
        float t = 0;

        player.LockIntoPushing();

        Vector3 crossProd = Vector3.Cross(direction, transform.right);
        bool isForward = crossProd.y > 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + (direction * distanceToMove);

        Vector3 startRot = _logTransform.localEulerAngles;
        Vector3 endRot = _logTransform.localEulerAngles;
        endRot.x += isForward ? 90 : -90;

        _numRolling ++;

        bool isFirstPush = true;

        while (t < 1)
        {
            t += Time.deltaTime * _pushSpeed;
            Vector3 prevPosition = transform.position;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            _logTransform.localEulerAngles = Vector3.Lerp(startRot, endRot, t);

            Vector3 amountMoved = transform.position - prevPosition;
    
            if (isFirstPush)
                player.PushableMoved(amountMoved);
    
            yield return null;

            if (t >= 1)
            {
                SnapToGrid();

                if (isFirstPush)
                    player.UnlockPushing();

                if (CheckIfShouldFall())
                {
                    yield return new WaitForSeconds(0.1f);
                    
                    while (IsFalling)
                    {
                        Debug.Log("isFalling");
                        yield return null;
                    }
                }
                if (CheckIfCanMove(direction))
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
