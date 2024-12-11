using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
    private int value = 1;
    [SerializeField] AudioClip sfx;
    AudioSource audio;
    void Start()
    {
        Tween tweenRotation = transform.DOLocalRotate(new Vector3(0f, transform.rotation.y+180, 0f), 2f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        Tween tweenMotion = transform.DOLocalMoveY(transform.position.y+.25f, 1.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
        // audio = GetComponent<AudioSource>();
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
