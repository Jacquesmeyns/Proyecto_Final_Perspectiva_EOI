using UnityEngine;

public class DrawParentGizmoWhenSelected : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        this.transform.parent.SendMessage("OnDrawGizmosSelected");
    }
}