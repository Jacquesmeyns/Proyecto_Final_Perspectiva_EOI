using DG.Tweening;
using UnityEngine;

public class Structure : HidableObject
{
    MeshRenderer meshRenderer;
    protected void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public override void Appear()
    {
        meshRenderer.enabled = true;
        Color currentColor = material.color;
        Color invisibleColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        DOVirtual.Color(material.color, invisibleColor, timeToAppear, (value) =>
        {
            material.color = value;
        });
    }
    
    public override void Disappear()
    {
        Color currentColor = material.color;
        Color invisibleColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        DOVirtual.Color(material.color, invisibleColor, timeToDisappear, (value) =>
        {
            material.color = value;
        });
    }
    
    [ContextMenu("Hide Object")]
    public override void Hide()
    {
        meshRenderer.enabled = false;
    }
}
