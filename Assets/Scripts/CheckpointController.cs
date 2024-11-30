using System;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private CheckpointController nextCheckpoint;
    public Vector3 nextSpawnPosition => nextCheckpoint.spawnPosition.position;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gameManager.currentGravityChanger = this;
        }
    }
}
