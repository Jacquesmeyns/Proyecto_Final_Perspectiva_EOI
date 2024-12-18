using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class GravityChanger : HidableObject
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject iconGO;
    [SerializeField] public Transform nextSpawn;
    [SerializeField] public Transform nextCameraOrientation;
    [SerializeField] public Mesh gizmoMesh;
    
    [SerializeField] ParticleSystem mainVFX;
    [SerializeField] ParticleSystem backGlowVFX;

    public bool isPlayerInside;
    [SerializeField] private bool active = false;
    public bool Active => active;
    public GameObject IconGO => iconGO;
    // public bool OnlyUseOnce = false;
    // public bool activated = false;

    private float colorChangeTime = 1.5f;

    [SerializeField] private List<HidableObject> objectsToHide;
    [SerializeField] private List<Renderer> UberFXRenderersList;
    [SerializeField] public bool flipMovement = false;
    [SerializeField, Tooltip("Forces previous rotation to be kept")] public bool forceMaintainRotation;
    [SerializeField] private bool forceShow = false;
    public bool ForceMaintainRotation => forceMaintainRotation;

    private void Awake()
    {
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se comprueba orientación del primer checkpoint y se oculta si no coincide
        if (gameManager.currentCheckpoint.SpawnTransform.localRotation != nextSpawn.localRotation && !forceShow)
        {
            foreach (var objectToHide in objectsToHide)
            {
                objectToHide.Hide();
            }
        }
        if(!Active)
            HideCustomVFX();
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

    [ContextMenu("Activate")]
    public void Activate()
    {
        active = true;
        // ChangeMaterialToActive();
        PlayCustomVFX();
    }

    public void ShowHiddenObjects()
    {
        foreach (var hiddenObject in objectsToHide)
        {
            hiddenObject.Appear();
        }
    }

    public void DisappearHiddableObjects()
    {
        foreach (var hiddenObject in objectsToHide.Where(hiddenObject => hiddenObject!=null))
        {
            hiddenObject.Disappear();
        }
    }

    private void HideUberFXMaterials()
    {
        foreach (var renderer in UberFXRenderersList)
        {
            renderer.material.SetVector("_MainAlphaChannel", new Vector4(0, 0, 0, 0));
        }
    }

    private void AppearUberFXMaterials(float duration)
    {
        foreach (var renderer in UberFXRenderersList)
        {
            if(renderer.material.shader.name == "Piloto Studio/UberFX")
            {
                DOVirtual.Float(0f,
                    1f,
                    duration,
                    (_value) =>
                    {
                        renderer.material.SetVector("_MainAlphaChannel",
                            new Vector4(0,
                                0,
                                0,
                                _value));
                    });
            }
        }
    }
    
    private void DisappearUberFXMaterials(float duration)
    {
        foreach (var renderer in UberFXRenderersList)
        {
            if (renderer.material.shader.name == "Piloto Studio/UberFX")
            {
                DOVirtual.Float(1f,
                    0f,
                    duration,
                    (_value) =>
                    {
                        renderer.material.SetVector("_MainAlphaChannel",
                            new Vector4(0,
                                0,
                                0,
                                _value));
                    });
            }
        }
    }

    private void HideCustomVFX()
    {
        mainVFX.Stop();
        backGlowVFX.gameObject.SetActive(false);
        mainVFX.gameObject.SetActive(false);
    }
    
    private void PlayCustomVFX()
    {
        mainVFX.gameObject.SetActive(true);
        mainVFX.Play();
        backGlowVFX.gameObject.SetActive(true);
    }

    #region HiddableOverrides

    public override void Appear()
    {
        AppearUberFXMaterials(timeToAppear);
        if(Active)
            PlayCustomVFX();
    }

    public override void Disappear()
    {
        DisappearUberFXMaterials(timeToDisappear);
        HideCustomVFX();
    }

    [ContextMenu("Hide Object")]
    public override void Hide()
    {
        HideUberFXMaterials();
        HideCustomVFX();
    }

    #endregion
}