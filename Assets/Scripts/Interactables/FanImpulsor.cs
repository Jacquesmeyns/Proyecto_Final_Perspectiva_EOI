using System;
using UnityEngine;

public class FanImpulsor : HidableObject
{
    [SerializeField] float fanStrength = 1.5f;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(fanStrength * transform.up, ForceMode.Acceleration);
        }
    }
}
