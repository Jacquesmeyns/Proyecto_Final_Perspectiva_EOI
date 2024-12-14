using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class Coin : HidableObject
{
    [SerializeField] private int value = 1;
    [SerializeField] AudioClip sfx;
    AudioSource audio;
    void Start()
    {
        base.Start();
        Tween tweenRotation = transform.DOLocalRotate(new Vector3(0f, transform.localRotation.y+180, 0f), 2f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        Tween tweenMotion = transform.DOLocalMoveY(transform.position.y+.25f, 1.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // if(sfx != null)
            //     audio..PlayClipAtPoint(sfx, transform.position);
            // else
            // {
            //     
            // }
            other.GetComponent<PlayerController>().GameManager.UpdateScore(value);
            Destroy(gameObject);
        }
    }
}
