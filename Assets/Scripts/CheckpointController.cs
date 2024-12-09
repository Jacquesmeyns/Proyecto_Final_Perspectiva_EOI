using System;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Transform spawnPosition;
    public Vector3 SpawnPosition => spawnPosition.position;
    [SerializeField] private CheckpointController nextCheckpoint;
    public Vector3 nextSpawnPosition => nextCheckpoint.spawnPosition.position;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.currentCheckpoint = this;
        }
    }
}
