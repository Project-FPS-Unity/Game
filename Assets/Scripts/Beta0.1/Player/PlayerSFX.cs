using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource playerSteps;
    [SerializeField] private AudioSource playerFire;
    [SerializeField] private AudioSource playerReload;

    public void Step()
    {
        playerSteps.Play();
        playerFire.Stop();
        playerReload.Stop();
    }

    public void Fire()
    {
        playerFire.Play();
        playerReload.Stop();
    }

    public void StopFire()
    {
        playerFire.Stop();
    }

    public void Reload()
    {
        playerFire.Stop();
        playerReload.Play();
    }
}
