using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class GrindRail : MonoBehaviour
    {
        [SerializeField] private Transform endA;
        [SerializeField] private Transform endB;
        [SerializeField] private float width = 0.3f;
        [SerializeField] private float height = 0.7f;

        public Vector3 PositionA => endA.position;
        public Vector3 PositionB => endB.position;

        public Vector3 Forward => (PositionA - PositionB).normalized;

        public Quaternion Rotation => Quaternion.LookRotation(Forward, Vector3.up);

        // Start is called before the first frame update
        void Start()
        {
            CreateCollider();
        }

        void CreateCollider()
        {
            GameObject railTrigger = new GameObject("RailTrigger");
            railTrigger.transform.SetParent(transform);
            railTrigger.transform.position = Vector3.Lerp(endA.position, endB.position, 0.5f);

            GrindRailTrigger trigger = railTrigger.AddComponent<GrindRailTrigger>();
            trigger.Init(endA.position, endB.position, width, height, this);
        }


        public Vector3 FindClosestPoint(Vector3 point)
        {
            Vector3 lineDirection = PositionB - PositionA;
            float lineLength = lineDirection.magnitude;
            lineDirection.Normalize();

            float projectLength = Vector3.Dot(point - PositionA, lineDirection);
            projectLength = Mathf.Clamp(projectLength, 0, lineLength);

            return PositionA + lineDirection * projectLength;
        }

        public bool IsPositionOnRail(Vector3 point)
        {
            Vector3 lineDirection = PositionB - PositionA;
            float lineLength = lineDirection.magnitude;
            lineDirection.Normalize();

            float projectLength = Vector3.Dot(point - PositionA, lineDirection);

            if (projectLength < 0 || projectLength > lineLength)
                return false;

            return true;
        }

    }
}