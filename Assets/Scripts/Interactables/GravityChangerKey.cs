using System;
using UnityEngine;

public class GravityChangerKey : MonoBehaviour
{
    [SerializeField] GravityChanger associatedGravityChanger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            associatedGravityChanger.Activate();
            Destroy(gameObject);
        }
    }
}
