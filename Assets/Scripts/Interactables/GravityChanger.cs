using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject iconGO;
    [SerializeField] public Transform nextSpawn;
    [SerializeField] public Transform nextCameraOrientation;
    [SerializeField] public Mesh gizmoMesh;

    public bool isPlayerInside;
    [SerializeField] private bool active = false;
    public bool Active => active;
    private float colorChangeTime = 1.5f;

    [SerializeField] private List<HidableObject> objectsToHide;
    [SerializeField] public bool flipMovement = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se comprueba orientación del primer checkpoint y se oculta si no coincide
        if (gameManager.currentCheckpoint.transform.localRotation != nextSpawn.localRotation)
        {
            foreach (var objectToHide in objectsToHide)
            {
                objectToHide.Hide();
            }
        }
        Tween tween = iconGO.transform.DOLocalRotate(new Vector3(0f, iconGO.transform.rotation.y+180, 0f), 2f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
        iconGO.GetComponent<Renderer>().material.color = new Color(0.29f,0.31f,0.39f);
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

    public void ChangeMaterialToActive()
    {
        var iconMaterial = iconGO.GetComponent<Renderer>().material;
        DOVirtual.Color(iconMaterial.color, new Color(0.33f,0.20f,0.45f, 1f), colorChangeTime, (value) =>
        {
            iconMaterial.color = value;
        });
    }
    
    private void OnDrawGizmosSelected()
    {
        var defaultColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(gizmoMesh, nextSpawn.position, nextSpawn.rotation, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(gizmoMesh, nextCameraOrientation.position, nextCameraOrientation.rotation, Vector3.one);
        Gizmos.color = defaultColor;
        // Gizmos.DrawSphere(nextCameraOrientation.position, 0.2f);
        Gizmos.DrawLine(nextCameraOrientation.position, nextSpawn.position);
    }

    public void Activate()
    {
        active = true;
        ChangeMaterialToActive();
    }

    public void ShowHiddenObjects()
    {
        foreach (var hiddenObject in objectsToHide)
        {
            hiddenObject.Appear();
        }
    }
}