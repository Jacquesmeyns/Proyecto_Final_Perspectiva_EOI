using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] AudioClip sfx;
    AudioSource audio;
    private Material material;
    void Start()
    {
        Tween tweenRotation = transform.DOLocalRotate(new Vector3(0f, transform.localRotation.y+180, 0f), 2f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        Tween tweenMotion = transform.DOLocalMoveY(transform.position.y+.25f, 1.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
        // audio = GetComponent<AudioSource>();

        material = GetComponent<MeshRenderer>().material;
        material = new Material(GetComponent<MeshRenderer>().material);
    }

    [ContextMenu("Appear")]
    public void Appear()
    {
        material.SetFloat("_DissolveStrength",1f);
        DOVirtual.Float(0f, 1f, 1f, (_value) =>
        {
            material.SetFloat("_DissolveStrength",_value);
        });
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
