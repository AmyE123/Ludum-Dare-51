using System.Collections;
using System.Collections.Generic;
using ThirdPersonMovement;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    protected enum PushDirection { None, Forward, Right, Back, Left }

    [SerializeField]
    protected float _pushSpeed = 1;

    [SerializeField]
    protected Collider _collider;

    [SerializeField]
    private float _fallSpeed = 5;

    [SerializeField]
    private float _floatSpeed = 5;

    protected virtual bool CanMoveLeftRight => true;
    protected virtual bool CanMoveForwardBack => true;

    private float _fallHeight;
    private Vector3 _velocity;

    private Quaternion _rotation;

    private Rigidbody _rb;

    private WaterManager _water;

    public bool IsFalling => _rb.velocity.y < -0.01f;

    private bool _isInWater;

    public bool IsInWater => _isInWater;

    private void Start()
    {
        _rotation = transform.rotation;
        _rb = GetComponent<Rigidbody>();
        _water = FindObjectOfType<WaterManager>();
        SnapToGrid();
    }

    private void Update()
    {
        transform.rotation = _rotation;
        float topOfObject = transform.position.y + _collider.bounds.extents.y;
        _isInWater = _water.WaterHeightExact >= topOfObject - 0.2f;
        _rb.isKinematic = _isInWater;

        if (_isInWater)
        {
            float yPos = _water.WaterHeightExact - _collider.bounds.extents.y + 0.1f;
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
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

    public bool CheckIfShouldFall()
    {
        Bounds bounds = _collider.bounds;
        List<Vector3> hitOffsets = new List<Vector3>();

        float startX = bounds.extents.x - 0.5f;
        float startY = bounds.extents.z - 0.5f;

        for (int x=0; x<Mathf.RoundToInt(bounds.extents.x * 2); x++)
        {
            for (int y=0; y<Mathf.RoundToInt(bounds.extents.z * 2); y++)
            {
                hitOffsets.Add(new Vector3(x-startX, 0, y-startY));
            }
        }

        Vector3 mid = transform.position;
        mid -= new Vector3(0, bounds.extents.y-0.1f, 0);
        
        float highestPoint = -10f;

        foreach (Vector3 v in hitOffsets)
        {
            if (Physics.Raycast(mid + v, Vector3.down, out RaycastHit hit, 10))
            {
                if (hit.distance < 0.11f)
                    return false;
                
                highestPoint = Mathf.Max(highestPoint, hit.point.y);
            }
        }

        return true;
    }

    protected bool CheckIfCanMove(Vector3 direction)
    {
        PushDirection pushDir = GetPushDirection(direction);
        Bounds bounds = _collider.bounds;
        List<Vector3> hitOffsets = new List<Vector3>();

        if (pushDir == PushDirection.Left || pushDir == PushDirection.Right)
        {
            if (CanMoveLeftRight == false)
                return false;

            float startX = bounds.extents.z - 0.5f;
            float startY = bounds.extents.y - 0.5f;

            for (int x=0; x<Mathf.RoundToInt(bounds.extents.z * 2); x++)
            {
                for (int y=0; y<Mathf.RoundToInt(bounds.extents.y * 2); y++)
                {
                    hitOffsets.Add(new Vector3(0, y-startY, x-startX));
                }
            }
        }

        if (pushDir == PushDirection.Forward || pushDir == PushDirection.Back)
        {
            if (CanMoveForwardBack == false)
                return false;

            float startZ = bounds.extents.x - 0.5f;
            float startY = bounds.extents.y - 0.5f;

            for (int x=0; x<Mathf.RoundToInt(bounds.extents.x * 2); x++)
            {
                for (int y=0; y<Mathf.RoundToInt(bounds.extents.y * 2); y++)
                {
                    hitOffsets.Add(new Vector3(x-startZ, y-startY, 0));
                }
            }
        }

        Vector3 mid = transform.position;

        if (pushDir == PushDirection.Left)      mid -= new Vector3(bounds.extents.x - 0.1f, 0, 0);
        if (pushDir == PushDirection.Right)     mid += new Vector3(bounds.extents.x - 0.1f, 0, 0);
        if (pushDir == PushDirection.Forward)   mid += new Vector3(0, 0, bounds.extents.z - 0.1f);
        if (pushDir == PushDirection.Back)      mid -= new Vector3(0, 0, bounds.extents.z - 0.1f);

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

    protected void SnapToGrid()
    {
        bool halfX = Mathf.RoundToInt(_collider.bounds.extents.x * 2) % 2 == 1;
        bool halfZ = Mathf.RoundToInt(_collider.bounds.extents.z * 2) % 2 == 1;
        
        float xPos = transform.position.x;
        float zPos = transform.position.z;

        if (halfX) xPos += 0.5f;
        if (halfZ) zPos += 0.5f;

        xPos = Mathf.Round(xPos);
        zPos = Mathf.Round(zPos);

        if (halfX) xPos -= 0.5f;
        if (halfZ) zPos -= 0.5f;

        transform.position = new Vector3(xPos, transform.position.y, zPos);
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

        SnapToGrid();
        player.UnlockPushing();
    }
}
