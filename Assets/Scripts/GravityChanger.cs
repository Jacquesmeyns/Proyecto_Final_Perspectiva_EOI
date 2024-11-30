using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject iconGO;
    [SerializeField] public Transform nextSpawn;
    [SerializeField] public Mesh gizmoMesh;

    public bool isPlayerInside;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tween tween = iconGO.transform.DOLocalRotate(new Vector3(0f, iconGO.transform.rotation.y+180, 0f), 2f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.currentGravityChanger = this;
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawMesh(gizmoMesh, nextSpawn.position, nextSpawn.rotation, Vector3.one);
    }
}
