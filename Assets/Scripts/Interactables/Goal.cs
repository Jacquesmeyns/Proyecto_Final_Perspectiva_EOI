using System;
using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            particleSystem.Play();
            gameManager.onGoalEnd.Invoke(transform);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    
}
