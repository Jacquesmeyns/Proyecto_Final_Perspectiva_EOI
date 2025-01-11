using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ButtonUiController : MonoBehaviour
{
    //private IEnumerator currentButtonCoroutine;
    private Tween hoverTween;
    
    private void Awake()
    {
        // currentButtonCoroutine = HoverCoroutine();
        hoverTween = null;
    }

    public void CursorOn()
    {
        hoverTween = transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        // StartCoroutine(currentButtonCoroutine);
    }

    // private IEnumerator HoverCoroutine()
    // {
    //     
    // }
    
    public void CursorOff()
    {
        // StopCoroutine(currentButtonCoroutine);
        hoverTween.Kill();
        transform.localScale = Vector3.one;
    }
}
