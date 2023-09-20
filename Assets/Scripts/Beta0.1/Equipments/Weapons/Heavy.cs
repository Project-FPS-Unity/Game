using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Equipment
{
    [Header("Camara")]
    [SerializeField] private Camera fpsCam;
    [Header("Gun Details")]
    [Header("VFX")]
    [SerializeField] private ParticleSystem muzzleFlash;
    // Ammo
    private int maxAmmo = 50;
    private int currentAmmo;
    // Magazine
    private int maxMagazine = 5;
    private int currentMagazine;

    private float damage = 20f;
    private float range = 10f;
    private float reloadTime = 1f;
    private bool isReloading = false;

    //Animation
    private Animator animator;

    private void OnEnable()
    {
        isReloading = false;
    }

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Override
    protected override void Action()
    {
        if (isReloading) return;
        CheckShoot();
    }
    protected override IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReload", true);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        animator.SetBool("isReload", false);
        isReloading = false;
    }

    protected override void InitAmmo()
    {
        currentAmmo = maxAmmo;
    }

    // Equipment Action
    private void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;
        Debug.Log("current ammo : " + currentAmmo);
        RaycastHit hit;
        bool isHit;
        isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (isHit)
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
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
}
