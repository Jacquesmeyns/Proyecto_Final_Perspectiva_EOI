using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spikes : HidableObject
{
    private List<MeshRenderer> childMeshes;
    private BoxCollider boxCollider;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        childMeshes = new List<MeshRenderer>();
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            childMeshes.Add(mesh);
        }
    }

    public override void Appear()
    {
        boxCollider.enabled = true;
        foreach (var mesh in childMeshes)
        {
            DOVirtual.Float(1f, 0f, timeToAppear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
        }
    }

    public override void Disappear()
    {
        boxCollider.enabled = false;
        foreach (var mesh in childMeshes)
        {
            DOVirtual.Float(0f, 1f, timeToDisappear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
        }
    }

    [ContextMenu("Hide Object")]
    public override void Hide()
    {
        boxCollider.enabled = false;
        foreach (var mesh in childMeshes)
        {
            mesh.material.SetFloat("_DissolveStrength", 1f);
        }
    }
}
