using DG.Tweening;
using UnityEngine;

namespace Interactables.Hidables
{
    [System.Serializable]
    public class HidableMaterial : MonoBehaviour, IHidableObject
    {
        protected Material material;
        [SerializeField] protected float timeToAppear;
        [SerializeField] protected float timeToDisappear;
        
        protected void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        public void HidableSetUp()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Appear()
        {
            DOVirtual.Float(1f, 0f, timeToAppear, (_value) =>
            {
                material.SetFloat("_DissolveStrength",_value);
            });
        }
        
        public virtual void Disappear()
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
}