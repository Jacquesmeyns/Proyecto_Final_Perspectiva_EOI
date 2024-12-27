using UnityEngine;

namespace Interactables.Hidables
{
    [System.Serializable]
    public class HidableGameObject : MonoBehaviour, IHidableObject
    {
        public void HidableSetUp()
        {
            //Not needed
        }

        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu("Hide Object")]
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}