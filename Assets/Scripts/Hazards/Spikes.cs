using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spikes : HidableObject
{
    private List<MeshRenderer> childMeshes;
    void Awake()
    {
        childMeshes = new List<MeshRenderer>();
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            childMeshes.Add(mesh);
        }
    }

    public override void Appear()
    {
        foreach (var mesh in childMeshes)
        {
            DOVirtual.Float(1f, 0f, timeToAppear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
        }
    }

    public override void Disappear()
    {
        foreach (var mesh in childMeshes)
        {
            DOVirtual.Float(0f, 1f, timeToDisappear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
        }
    }

    [ContextMenu("Hide Object")]
    public override void Hide()
    {
        foreach (var mesh in childMeshes)
        {
            mesh.material.SetFloat("_DissolveStrength", 1f);
        }
    }
}
