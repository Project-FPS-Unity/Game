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

    private float reloadTime = 1f;
    private bool isReloading = false;

    //Animation
    private Animator animator;

    [Header("Reference")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject objectToFire;

    [Header("Settings")]
    [SerializeField] private float fireCooldown;
    private int maxAmmo = 100;
    private int currentAmmo;

    [Header("Throwing")]
    [SerializeField] private float bulletForce;
    [SerializeField] private float bulletUpwardForce;
    private bool readyToFire;

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
        readyToFire = true;
        currentAmmo = maxAmmo;
    }

    // Equipment Action
    private void Shoot()
    {
        muzzleFlash.Play();
        FireBullet();
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

        if (Input.GetMouseButton(0) && readyToFire && currentAmmo > 0)
        {
            Debug.Log("Fire!");
            Shoot();
        }
    }

    private void FireBullet()
    {
        readyToFire = false;

        // Insatantiate object to throw
        GameObject projectile = Instantiate(objectToFire, attackPoint.position, cam.rotation);

        // Get Rigidbody componenet
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Calculate direction
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // Add Force
        Vector3 forceToAdd = forceDirection * bulletForce + transform.up * bulletUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        currentAmmo--;

        // Implement throw cooldown
        Invoke(nameof(ResetFire), fireCooldown);
    }

    private void ResetFire()
    {
        readyToFire = true;
    }
}
