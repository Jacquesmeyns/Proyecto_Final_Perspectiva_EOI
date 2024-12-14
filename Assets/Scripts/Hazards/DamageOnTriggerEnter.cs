using System;
using UnityEngine;

public class DamageOnTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.TakeDamage();
        }
    }
}
