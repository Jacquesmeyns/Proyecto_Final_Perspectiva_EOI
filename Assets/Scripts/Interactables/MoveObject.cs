using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private Vector3 Start, End;
    [SerializeField] private float moveDistance;
    [SerializeField] private float Speed = 0.5f;
    private float Time => Mathf.Abs(moveDistance / Speed);
    
    // private IEnumerator moveLoop;
    private Rigidbody rb;
    // private float rewind;
    //private Vector3 direction;


    private void Awake()
    {
        Start = transform.position;
        End = Start + moveDistance * transform.forward;
        //direction = (End - Start).normalized;
        rb = GetComponent<Rigidbody>();

        // rewind = Start.magnitude > End.magnitude ? 1 : -1;
        
        rb.DOMove(End, Time).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        // moveLoop = MoveCoroutine();
        // StartCoroutine(moveLoop);
    }

    // private void FixedUpdate()
    // {
    //     rb.MovePosition(transform.position + direction * Time.deltaTime * Speed * rewind);
    // }

    // private IEnumerator MoveCoroutine()
    // {
    //     while (true)
    //     {
    //         while (rb.position.magnitude >= End.magnitude)
    //         {
    //             yield return new WaitForEndOfFrame();
    //         }
    //
    //         rewind = -1;
    //
    //         while (rb.position.magnitude <= Start.magnitude)
    //         {
    //             yield return new WaitForEndOfFrame();
    //         }
    //         
    //         rewind = 1;
    //     }
    // }
    
    private void OnDrawGizmosSelected()
    {
        //Draw expectedEnd
        Gizmos.DrawLine(transform.position, transform.position + moveDistance * transform.forward);
    }
}
