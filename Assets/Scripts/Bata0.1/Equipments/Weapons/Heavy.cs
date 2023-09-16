using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Equipment
{
    [Header("Camara")]
    [SerializeField] private Camera FPSCam;
    [Header("Gun Details")]
    [Header("VFX")]
    [SerializeField] private ParticleSystem MuzzleFlash;
    // Ammo
    private int maxAmmo = 50;
    private int currentAmmo;
    // Magazine
    private int maxMagazine = 5;
    private int currentMagazine;

    private float Damage = 20f;
    private float range = 10f;
    private float reloadTime = 1f;
    private bool isReloading = false;

    // Override
    protected override void Action()
    {
        if (isReloading) return;
        CheckShoot();
    }
    protected override IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    protected override void InitAmmo()
    {
        currentAmmo = maxAmmo;
    }

    // Equipment Action
    private void Shoot()
    {
        MuzzleFlash.Play();
        currentAmmo--;
        Debug.Log("current ammo : " + currentAmmo);
        RaycastHit hit;
        bool isHit;
        isHit = Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range);
        if (isHit)
        {
            Debug.Log(hit.transform.name);
            //    Target target = hit.transform.GetComponent<Target>();
            //    if (target != null)
            //    {
            //        target.TakeDamage(Damage);
            //    }
        }
        else
        {
            Debug.Log("Not hit");
        }
    }

    private void CheckShoot()
    {
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
    private void OnEnable()
    {
        isReloading = false;
    }
}
