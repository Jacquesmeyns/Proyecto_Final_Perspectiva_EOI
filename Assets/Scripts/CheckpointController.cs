using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckpointController : HidableObject
{
    [SerializeField] public Mesh gizmoMesh;
    [SerializeField] GameManager gameManager;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] ParticleSystem idleParticles;
    [SerializeField] ParticleSystem activationParticles;
    [SerializeField] List<ParticleSystem> RayCenters;
    [SerializeField] ParticleSystem FireCenter;
    
    public Vector3 SpawnPosition => spawnPosition.position;
    public Transform SpawnTransform => spawnPosition;
    private void Awake()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var ray in RayCenters)
            {
                ray.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            FireCenter.Stop();
            gameManager.currentCheckpoint = this;
            activationParticles.gameObject.SetActive(true);
            activationParticles.Play(true);
            
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
    
    #region HiddableOverrides

    public override void Appear()
    {
        gameObject.SetActive(true);
    }

    public override void Disappear()
    {
        gameObject.SetActive(false);
    }

    [ContextMenu("Hide Object")]
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
