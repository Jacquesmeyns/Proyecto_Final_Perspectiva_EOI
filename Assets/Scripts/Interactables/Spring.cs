using System;
using UnityEngine;

namespace Interactables
{
    public class Spring : MonoBehaviour
    {
        SpringJoint spring;
        [SerializeField] private float minSpring = 10;
        [SerializeField] private float maxSpring = 300;

        private void Start()
        {
            spring = GetComponentInParent<SpringJoint>();
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
    }
}