using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveSaw : MonoBehaviour
{
    private Vector3 Start, End;
    [SerializeField] private float moveDistance;
    [SerializeField] private float TimeToCompleteMove = 1f;
    [SerializeField] private float TimeToCompleteRotation = 1f;
    
    private IEnumerator moveLoop;
    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.right;
    //private float rewind;
    //private Vector3 direction;


    private void Awake()
    {
        Start = transform.position;
        End = Start + moveDistance * moveDirection;
        //direction = (End - Start).normalized;
        rb = GetComponent<Rigidbody>();
        transform.position = Start;

        //rewind = Start.magnitude > End.magnitude ? 1 : -1;
        
        rb.DOMove(End, TimeToCompleteMove).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        rb.DORotate(new Vector3(0f, 0f, 360f), TimeToCompleteRotation, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart);
        // moveLoop = MoveCoroutine();
        // StartCoroutine(moveLoop);
    }

    private void FixedUpdate()
    {
        //rb.MovePosition(transform.position + direction * Time.deltaTime * Speed * rewind);
    }

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
        Gizmos.DrawLine(transform.position, transform.position + moveDistance * moveDirection);
    }
}
