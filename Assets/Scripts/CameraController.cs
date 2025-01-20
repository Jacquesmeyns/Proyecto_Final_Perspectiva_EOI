using System;
using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private Transform dollyTransform;
    [SerializeField] public CinemachineCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
    public CinemachineHardLockToTarget LockToTarget;
    //[SerializeField] private CinemachineCameraOffset vcamOffset;
    private Transform target;
    private Vector3 newDollyPos;
    private int currentAxis;    //0 = x, 1 = y, 2 = z
    private Vector3 currentDirection;
    private Vector3 currentOffset;
    private float distanceOffset = 1;
    private float tweenDuration = 1f;

    [SerializeField] float shakeIntensity = 5f;
    [SerializeField] float shakeDuration = 0.5f;

    private void Start()
    {
        LockToTarget = vcam.GetComponent<CinemachineHardLockToTarget>();
        vcam.Lens.NearClipPlane = -15;
        target = vcam.Target.TrackingTarget;
        currentOffset = target.position + Vector3.back * distanceOffset;
        currentDirection = transform.forward;
        noise = vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        vcam.transform.position = target.position + currentOffset;
    }


    /// <summary>
    /// Tweens to a new lookDirection maintaning an offset from the target player.
    /// </summary>
    /// <param name="newDirection"></param>
    public void MoveAndLookAtNewDir(GravityChanger gravityChanger)
    {
        var newDirection = (gravityChanger.nextSpawn.position -
                           gravityChanger.nextCameraOrientation.position).normalized;
        //var newPosition = gravityChanger.nextSpawn.position;
        var newRotation = gravityChanger.nextCameraOrientation.rotation;
        
        //next rotation
        if (!gravityChanger.ForceMaintainRotation)
        {
            //var nextRotation = Quaternion.LookRotation(newDirection);
            vcam.transform.DORotateQuaternion(newRotation, tweenDuration)
                .SetEase(Ease.InOutCubic);
        }

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

    public void CameraShake()
    {
        StartCoroutine(ShakeCoroutine(shakeIntensity, shakeDuration));
    }
    
    private IEnumerator ShakeCoroutine(float shakeIntensity, float shakeTiming)
    {
        Noise(5, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain) {
        noise.AmplitudeGain = amplitudeGain;
        noise.FrequencyGain = frequencyGain;     
    }
}
