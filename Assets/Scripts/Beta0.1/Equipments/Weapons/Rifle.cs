using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Equipment
{
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

    protected override void Action()
    {
        if (Input.GetMouseButton(0) && readyToFire && currentAmmo > 0)
        {
            FireBullet();
        }
    }

    protected override void InitAmmo()
    {
        readyToFire = true;
        currentAmmo = maxAmmo;
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
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
        Invoke(nameof(ReadyToFire), fireCooldown);
    }

    private void ReadyToFire()
    {
        readyToFire = true;
    }

}
