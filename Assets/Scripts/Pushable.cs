using System.Collections;
using System.Collections.Generic;
using ThirdPersonMovement;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    [SerializeField]
    private float _pushSpeed = 1;

    public virtual void Push(Vector3 direction, PersonPushController player)
    {
        StartCoroutine(PushRoutine(direction, player));
    }

    protected virtual IEnumerator PushRoutine(Vector3 direction, PersonPushController player)
    {
        float distanceToMove = 1f;
        float t = 0;

        player.LockIntoPushing();

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + (direction * distanceToMove);

        while (t < 1)
        {
            t += Time.deltaTime * _pushSpeed;
            Vector3 prevPosition = transform.position;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            Vector3 amountMoved = transform.position - prevPosition;
            player.PushableMoved(amountMoved);
            yield return null;
        }

        player.UnlockPushing();
    }
}
