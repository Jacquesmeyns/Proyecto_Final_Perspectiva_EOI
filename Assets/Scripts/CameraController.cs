using System;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private Transform dollyTransform;
    [SerializeField] public CinemachineCamera vcam;
    //[SerializeField] private CinemachineCameraOffset vcamOffset;
    private Transform target;
    private Vector3 newDollyPos;
    private int currentAxis;    //0 = x, 1 = y, 2 = z
    private Vector3 currentDirection;
    private Vector3 currentOffset;
    private float distanceOffset = 1;
    private float tweenDuration = 1f;

    private void Start()
    {
        vcam.Lens.NearClipPlane = -15;
        target = vcam.Target.TrackingTarget;
        currentOffset = target.position + Vector3.back * distanceOffset;
        currentDirection = transform.forward;
    }

    private void Update()
    {
        vcam.transform.position = target.position + currentOffset;
    }


    /// <summary>
    /// Tweens to a new lookDirection maintaning an offset from the target player.
    /// </summary>
    /// <param name="newDirection"></param>
    /// TODO La cámara funciona bien pero si se pone desde un vector opuesto al usual, los controles no se invierten acorde a la nueva perspectiva, habrá que controlar la inversión de inputs
    public void MoveAndLookAtNewDir(Vector3 newDirection)
    {
        //next rotation
        var nextRotation = Quaternion.LookRotation(newDirection);
        vcam.transform.DORotateQuaternion(nextRotation, tweenDuration)
            .SetEase(Ease.InOutCubic);
        //Change position to new offset
        var newOffset = -newDirection * distanceOffset;
        changeOffset(newOffset);
    }

    //Depende de la dirección
    private void changeOffset(Vector3 newOffset)
    {
        DOTween.To(() => currentOffset, x => currentOffset = x, newOffset, duration: tweenDuration)
            .SetEase(Ease.InOutCubic);
        //currentOffset = newOffset;
    }
}
