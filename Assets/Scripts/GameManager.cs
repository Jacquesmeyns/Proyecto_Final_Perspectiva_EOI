using System;
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
    public CheckpointController currentCheckpoint;

    private void Awake()
    {
        if(currentCheckpoint == null)
            throw new Exception("No first checkpoint setted. Please assign one for the player Spawn point.");
        player.transform.position = currentCheckpoint.SpawnPosition;
    }

    public void ChangeToNewGravity()
    {
        var newGravityDirection = -currentGravityChanger.nextSpawn.up;
        newGravityDirection = Auxiliar.Round(newGravityDirection);
        if(newGravityDirection.x + newGravityDirection.y + newGravityDirection.z > 1)
            throw new Exception("The new gravity is not suitable for an isometric view. Two axis must be equal to zero.");

        if (GravityController.ChangeGravityDirection(newGravityDirection))
            SetConstraints(newGravityDirection);
        else
            FlipConstraints(newGravityDirection);
        player.Teleport(currentGravityChanger.nextSpawn.position);
        cameraController.MoveAndLookAtNewDir(currentGravityChanger.nextSpawn.position - currentGravityChanger.nextCameraOrientation.position);
    }

    private void FlipConstraints(Vector3 gravity)
    {
        if(Mathf.Abs(gravity.x) == 1)
            if (player.Rb.constraints == RigidbodyConstraints.FreezePositionZ)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
            else
                player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        else if(Mathf.Abs(gravity.y) == 1)
            if (player.Rb.constraints == RigidbodyConstraints.FreezePositionZ)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
            else
                player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
        else if(Mathf.Abs(gravity.z) == 1)
            if (player.Rb.constraints == RigidbodyConstraints.FreezePositionX)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
            else
                player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
    }

    public void SetConstraints(Vector3 newGravity)
    {
        var nextPointLocalRotation = Auxiliar.Round(currentGravityChanger.nextSpawn.transform.localRotation.eulerAngles);
        if (Mathf.Abs(newGravity.x) == 1)
        {
            if(Mathf.Abs(nextPointLocalRotation.x) == 90)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
            else if (nextPointLocalRotation.y == 0 ||
                     nextPointLocalRotation.y == 180)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
            else
                throw new Exception("The nextSpawn rotation is not suitable for an isometric view.");
        }
        else if (Mathf.Abs(newGravity.y) == 1)
        {
            if(Mathf.Abs(nextPointLocalRotation.y) == 90)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionZ;
            else if (nextPointLocalRotation.y == 0 ||
                     nextPointLocalRotation.y == 180)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
            else
                throw new Exception("The nextSpawn rotation is not suitable for an isometric view.");
        }
        else if (Mathf.Abs(newGravity.z) == 1)
        {
            if(Mathf.Abs(nextPointLocalRotation.x) == 90)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionX;
            else if (Mathf.Abs(nextPointLocalRotation.y) == 90)
                player.Rb.constraints = RigidbodyConstraints.FreezePositionY;
            else
                throw new Exception("The nextSpawn rotation is not suitable for an isometric view.");
        }
    }
}