using System.Collections;
using System.Collections.Generic;
using ThirdPersonMovement;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    protected enum PushDirection { None, Forward, Right, Back, Left }

    [SerializeField]
    private float _pushSpeed = 1;

    [SerializeField]
    private Collider _collider;

    [SerializeField]
    private float _fallSpeed = 5;

    private bool _isFalling;
    private float _fallHeight;
    private Vector3 _velocity;

    public bool IsFalling => _isFalling;

    public bool IsInWater => false;

    private void Update()
    {
        if (_isFalling)
        {
            _velocity += new Vector3(0, -_fallSpeed, 0) * Time.deltaTime;
            transform.position += _velocity * Time.deltaTime;

            if (transform.position.y < _fallHeight)
            {
                transform.position = new Vector3(transform.position.x, _fallHeight, transform.position.z);
                _isFalling = false;
            }
        }
    }

    public virtual void Push(Vector3 direction, PersonPushController player)
    {
        if (CheckIfCanMove(direction) == false)
        {
            player.CancelPushing();
            return;
        }

        StartCoroutine(PushRoutine(direction, player));
    }

    public bool ShouldFall()
    {
        Bounds bounds = _collider.bounds;
        List<Vector3> hitOffsets = new List<Vector3>();

        float startX = bounds.extents.x - 0.5f;
        float startY = bounds.extents.z - 0.5f;

        for (int x=0; x<bounds.extents.x * 2; x++)
        {
            for (int y=0; y<bounds.extents.z * 2; y++)
            {
                hitOffsets.Add(new Vector3(x-startX, 0, y-startY));
            }
        }

        Vector3 mid = transform.position;
        mid -= new Vector3(0, bounds.extents.y, 0);
        
        float highestPoint = -10f;

        foreach (Vector3 v in hitOffsets)
        {
            if (Physics.Raycast(mid + v, Vector3.down, out RaycastHit hit, 10))
            {
                if (hit.distance < 0.01f)
                    return false;
                
                highestPoint = Mathf.Max(highestPoint, hit.point.y);
            }
        }

        _fallHeight = highestPoint + bounds.extents.y;
        _isFalling = true;
        return true;
    }

    protected bool CheckIfCanMove(Vector3 direction)
    {
        PushDirection pushDir = GetPushDirection(direction);
        Bounds bounds = _collider.bounds;
        List<Vector3> hitOffsets = new List<Vector3>();

        if (pushDir == PushDirection.Left || pushDir == PushDirection.Right)
        {
            float startX = bounds.extents.z - 0.5f;
            float startY = bounds.extents.y - 0.5f;

            for (int x=0; x<bounds.extents.z * 2; x++)
            {
                for (int y=0; y<bounds.extents.y * 2; y++)
                {
                    hitOffsets.Add(new Vector3(0, y-startY, x-startX));
                }
            }
        }

        if (pushDir == PushDirection.Forward || pushDir == PushDirection.Back)
        {
            float startZ = bounds.extents.x - 0.5f;
            float startY = bounds.extents.y - 0.5f;

            for (int x=0; x<bounds.extents.x * 2; x++)
            {
                for (int y=0; y<bounds.extents.y * 2; y++)
                {
                    hitOffsets.Add(new Vector3(x-startZ, y-startY, 0));
                }
            }
        }

        Vector3 mid = transform.position;

        if (pushDir == PushDirection.Left)      mid -= new Vector3(bounds.extents.x, 0, 0);
        if (pushDir == PushDirection.Right)     mid += new Vector3(bounds.extents.x, 0, 0);
        if (pushDir == PushDirection.Forward)   mid += new Vector3(0, 0, bounds.extents.z);
        if (pushDir == PushDirection.Back)      mid -= new Vector3(0, 0, bounds.extents.z);

        foreach (Vector3 v in hitOffsets)
        {
            if (Physics.Raycast(mid + v, direction, out RaycastHit hit, 0.8f))
            {
                return false;
            }
        }
        
        return true;
    }

    protected PushDirection GetPushDirection(Vector3 dir)
    {
        if (dir.x > 0.1f)
            return PushDirection.Right;
        if (dir.x < -0.1f)
            return PushDirection.Left;
        if (dir.z > 0.1f)
            return PushDirection.Forward;
        if (dir.z < -0.1f)
            return PushDirection.Back;

        return PushDirection.None;
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
