using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveSaw : HidableObject
{
    private Vector3 Start, End;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationRPM;
    [SerializeField] private LineRenderer line;
    private float timeToCompleteMove = 1f;
    private float timeToCompleteRotation = 1f;
    
    private IEnumerator moveLoop;
    private Rigidbody rb;

    private void Awake()
    {
        timeToCompleteMove = Mathf.Abs(moveDistance / moveSpeed);
        timeToCompleteRotation = Mathf.Abs(1f/(rotationRPM / 60f)); //60 seg in 1 min ||| freq = 1 / T (rotation period)
        Start = transform.position;
        End = Start + moveDistance * transform.right;
        rb = GetComponent<Rigidbody>();
        
        rb.DOMove(End, timeToCompleteMove).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        rb.DORotate(new Vector3(0f, 0f, 360f), timeToCompleteRotation, RotateMode.LocalAxisAdd)
            .SetLoops(-1).SetEase(Ease.Linear);

        var StartEnd = End - Start;
        line.SetPositions(new Vector3[]{Start, Start + (StartEnd*0.01f), Start + (StartEnd*0.99f), End});
        Hide();
    }

    private void OnDrawGizmosSelected()
    {
        //Draw expectedEnd
        Gizmos.DrawLine(transform.position, transform.position + moveDistance * transform.right);
    }

    public override void Appear()
    {
        StartCoroutine(DoFunctionAfter(0.5f, Show));
    }

    public override void Disappear()
    {
        Hide();
    }

    IEnumerator DoFunctionAfter(float time, Action function)
    {
        yield return new WaitForSeconds(time);
        function?.Invoke();
    }
    
    public override void Hide()
    {
        line.enabled = false;
    }

    private void Show()
    {
        line.enabled = true;
    }
}
