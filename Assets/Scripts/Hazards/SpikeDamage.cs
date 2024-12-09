using System;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
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
