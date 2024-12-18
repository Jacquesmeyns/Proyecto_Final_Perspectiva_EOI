using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationTarget = new Vector3(0f, 0f, 360f);
    [SerializeField] private float TimeToRotate = 0.5f;
    [SerializeField] private RotateMode rotateMode = RotateMode.LocalAxisAdd;
    [SerializeField] private LoopType loopType = LoopType.Incremental;
    [SerializeField] private Ease ease = Ease.Linear;
    private void Awake()
    {
        transform.DORotate(rotationTarget, TimeToRotate, rotateMode)
            .SetLoops(-1, loopType).SetEase(ease);
    }
}
