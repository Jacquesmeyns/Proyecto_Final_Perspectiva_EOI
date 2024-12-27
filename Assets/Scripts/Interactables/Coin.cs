using System;
using DG.Tweening;
using Interactables;
using Interactables.Hidables;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class Coin : HidableMaterial
{
    [SerializeField] private int value = 1;
    [SerializeField] GameObject coinModel;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioSource audioSource;
    void Awake()
    {
        Tween tweenRotation = coinModel.transform.DOLocalRotate(new Vector3(0f, 0f, transform.localRotation.z+180), 2f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        Tween tweenMotion = coinModel.transform.DOLocalMoveY(coinModel.transform.localPosition.y+.25f, 1.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);

        material = coinModel.GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            other.GetComponent<PlayerController>().GameManager.UpdateScore(value);
            GetComponent<SphereCollider>().enabled = false;
            coinModel.GetComponent<MeshRenderer>().enabled = false;
            explosionParticles.Play(true);
        }
    }
}
