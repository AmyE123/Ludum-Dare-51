using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonMovement
{
    public class GrindRailTrigger : MonoBehaviour
    {
        private GrindRail _rail;

        private Dictionary<Collider, PersonGrindController> _personLookup = new Dictionary<Collider, PersonGrindController>();

        public void Init(Vector3 endA, Vector3 endB, float width, float height, GrindRail rail)
        {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(width, height, Vector3.Distance(endA, endB));
            col.center = new Vector3(0, height / 2, 0);
            col.isTrigger = true;

            Vector3 alignmentVector = (endA - endB).normalized;
            transform.rotation = Quaternion.LookRotation(alignmentVector, Vector3.up);

            _rail = rail;
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player")
                return;

            if (_personLookup.ContainsKey(other) == false)
                _personLookup[other] = other.GetComponent<PersonGrindController>();
            
            _personLookup[other]?.EnterRail(_rail);
        }
    }
}