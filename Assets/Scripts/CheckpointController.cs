using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] public Mesh gizmoMesh;
    [SerializeField] GameManager gameManager;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] ParticleSystem idleParticles;
    [SerializeField] ParticleSystem activationParticles;
    [SerializeField] List<ParticleSystem> RayCenters;
    [SerializeField] ParticleSystem FireCenter;
    
    // private List<ParticleSystem> activeParticles;
    public Vector3 SpawnPosition => spawnPosition.position;
    public Transform SpawnTransform => spawnPosition;
    //[SerializeField] private CheckpointController nextCheckpoint;
    //public Vector3 nextSpawnPosition => nextCheckpoint.spawnPosition.position;

    private void Awake()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
        // activeParticles = new List<ParticleSystem>();
        //
        // foreach (var particleSystem in idleParticles.gameObject.GetComponentsInChildren<ParticleSystem>())
        // {
        //     if (particleSystem.isPlaying)
        //     {
        //         activeParticles.Add(particleSystem);
        //     }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // var idleParticlesMain = idleParticles.main;
            // idleParticlesMain.loop = false;
            // idleParticles.Stop();
            foreach (var ray in RayCenters)
            {
                ray.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            FireCenter.Stop();
            
            gameManager.currentCheckpoint = this;
            activationParticles.gameObject.SetActive(true);
            activationParticles.Play(true);
            // foreach (var particleSystem in activationParticles.gameObject.GetComponentsInChildren<ParticleSystem>())
            // {
            //     particleSystem.Play();
            // }
            
            GetComponent<BoxCollider>().enabled = false;
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
