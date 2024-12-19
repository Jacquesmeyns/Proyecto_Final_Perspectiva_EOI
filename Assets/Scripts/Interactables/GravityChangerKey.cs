using System;
using UnityEngine;

public class GravityChangerKey : MonoBehaviour
{
    [SerializeField] GravityChanger associatedGravityChanger;
    [SerializeField] ParticleSystem edgeVFX;
    [SerializeField] ParticleSystem mainVFX;
    [SerializeField] ParticleSystem backGlowVFX;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        var target = associatedGravityChanger.IconGO.transform;
        var rotation = Quaternion.FromToRotation(Vector3.up, target.position - edgeVFX.transform.position);
        edgeVFX.transform.rotation = rotation;
        // edgeVFX.transform.LookAt(target.position, Vector3.down);
        var edgeVFXMain = edgeVFX.main;
        var lifeTime = Vector3.Distance(edgeVFX.transform.position, target.position)
                       / edgeVFXMain.startSpeedMultiplier;
        edgeVFXMain.startLifetime = lifeTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            associatedGravityChanger.SetActive();
            GetComponent<SphereCollider>().enabled =false;
            mainVFX.Stop();
            edgeVFX.Stop();
            backGlowVFX.gameObject.SetActive(false);
        }
    }
}
