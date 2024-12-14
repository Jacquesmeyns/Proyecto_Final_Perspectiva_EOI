using System;
using DG.Tweening;
using UnityEngine;

public class HidableObject : MonoBehaviour
{
    private Material material;
    [SerializeField] private float timeToAppear;
    [SerializeField] private float timeToDisappear;

    protected void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public virtual void Appear()
    {
        DOVirtual.Float(1f, 0f, timeToAppear, (_value) =>
        {
            material.SetFloat("_DissolveStrength",_value);
        });
    }

    public void Disappear()
    {
        DOVirtual.Float(0f, 1f, timeToDisappear, (_value) =>
        {
            material.SetFloat("_DissolveStrength",_value);
        });
    }

    [ContextMenu("Hide Object")]
    public virtual void Hide()
    {
        material.SetFloat("_DissolveStrength", 1f);
    }
}
