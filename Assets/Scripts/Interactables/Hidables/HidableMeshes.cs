using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Interactables.Hidables
{
    /// <summary>
    /// Also manages colliders enabling and disabling when showing or hiding
    /// </summary>
    [System.Serializable]
    public class HidableMeshes : MonoBehaviour, IHidableObject
    {
        private BoxCollider boxCollider;
        private List<MeshRenderer> childMeshes;
        [SerializeField] protected float timeToAppear;
        [SerializeField] protected float timeToDisappear;

        void Awake()
        {
            HidableSetUp();
        }

        //TODO revisar lógica, por qué oculta también colliders?
        public void HidableSetUp()
        {
            boxCollider = GetComponent<BoxCollider>();
            childMeshes = new List<MeshRenderer>();
            foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
            {
                childMeshes.Add(mesh);
            }
        }

        public void Appear()
        {
            boxCollider.enabled = true;
            foreach (var mesh in childMeshes)
            {
                DOVirtual.Float(1f, 0f, timeToAppear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
            }
        }

        public void Disappear()
        {
            boxCollider.enabled = false;
            foreach (var mesh in childMeshes)
            {
                DOVirtual.Float(0f, 1f, timeToDisappear, (_value) => { mesh.material.SetFloat("_DissolveStrength", _value); });
            }
        }

        [ContextMenu("Hide Object")]
        public void Hide()
        {
            boxCollider.enabled = false;
            foreach (var mesh in childMeshes)
            {
                mesh.material.SetFloat("_DissolveStrength", 1f);
            }
        }
    }
}