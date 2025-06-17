using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = targetTransform.position;
    }
}
