using System;
using UnityEngine;

namespace Interactables
{
    public class Spring : MonoBehaviour
    {
        [SerializeField] SpringJoint spring;
        [SerializeField] private float minSpring = 10;
        [SerializeField] private float maxSpring = 300;
        private Vector3 platformPos, anchorPos;
        [SerializeField] private LineRenderer line;

        private void Update()
        {
            platformPos = spring.connectedBody.transform.position + spring.anchor*transform.parent.up.normalized.y;
            anchorPos = spring.transform.position;//StartPos + moveDistance * transform.right;
            var StartEnd = anchorPos - platformPos;

            line.SetPositions(new Vector3[]{platformPos, platformPos + (StartEnd*0.01f), platformPos + (StartEnd*0.99f), anchorPos});
        }
        
        private void FixedUpdate()
        {
            spring.connectedBody.AddForce(-transform.parent.up.normalized * spring.connectedBody.mass);
            //??
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Player")
            {
                spring.spring = maxSpring;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Player")
            {
                spring.spring = minSpring;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            //Draw expectedEnd
            platformPos = spring.connectedBody.transform.position + spring.anchor*transform.parent.up.normalized.y;
            anchorPos = spring.transform.position;//StartPos + moveDistance * transform.right;
            Gizmos.DrawLine( platformPos, anchorPos);
        }
    }
}