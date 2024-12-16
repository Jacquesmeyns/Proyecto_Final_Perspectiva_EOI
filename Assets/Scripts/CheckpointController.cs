using System;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] public Mesh gizmoMesh;
    [SerializeField] GameManager gameManager;
    [SerializeField] private Transform spawnPosition;
    public Vector3 SpawnPosition => spawnPosition.position;
    public Transform SpawnTransform => spawnPosition;
    //[SerializeField] private CheckpointController nextCheckpoint;
    //public Vector3 nextSpawnPosition => nextCheckpoint.spawnPosition.position;

    private void Awake()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.currentCheckpoint = this;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        var defaultColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawMesh(gizmoMesh, spawnPosition.position, spawnPosition.rotation, Vector3.one);
        Gizmos.color = defaultColor;
    }
}
