using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] Vector3 sideCameraPosition;
    [SerializeField] Vector3 topCameraPosition;
    public GravityChanger currentGravityChanger;
    
    /*[ContextMenu("Change gravity to X")]
    private void EnableGravityOnX()
    {
        GravityController.ChangeGravityToX();
        
    }*/
    
    [ContextMenu("Change gravity to Y")]
    public void EnableGravityOnY()
    {
        
        GravityController.ChangeGravityToY();
        player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        player.teleport(currentGravityChanger.nextSpawn.position);
        // cameraController.transform.position = sideCameraPosition;
        // cameraController.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        cameraController.changeDirection(Vector3.forward);
    }
    
    [ContextMenu("Change gravity to Z")]
    public void EnableGravityOnZ()
    {
        GravityController.ChangeGravityToZ();
        player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
        player.teleport(currentGravityChanger.nextSpawn.position);
        // cameraController.transform.position = topCameraPosition;
        // cameraController.transform.rotation = Quaternion.LookRotation(Vector3.down);
        cameraController.changeDirection(Vector3.down);
    }
}
