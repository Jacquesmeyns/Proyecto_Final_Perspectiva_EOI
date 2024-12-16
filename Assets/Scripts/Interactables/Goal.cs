using System;
using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            particleSystem.Play();
            gameManager.EndLevel(transform);
        }
    }

    
}
