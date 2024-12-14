using System.Collections.Generic;
using UnityEngine;

public class Spikes : HidableObject
{
    private List<MeshRenderer> childMeshes;
    void Start()
    {
        childMeshes = new List<MeshRenderer>();
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            childMeshes.Add(mesh);
        }
    }

    public override void Hide()
    {
        foreach (var childMesh in childMeshes)
        {
            childMesh.enabled = false;
        }
    }

    public override void Appear()
    {
        //TODO arreglar con un material que haga dissolve
        foreach (var childMesh in childMeshes)
        {
            childMesh.enabled = true;
        }
    }
}
