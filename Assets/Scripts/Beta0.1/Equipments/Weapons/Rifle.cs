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
    private Bullet bullet;
    private float reloadTime = 1f;

    [Header("Throwing")]
    [SerializeField] private float bulletForce;
    [SerializeField] private float bulletUpwardForce;
    private bool readyToFire;

    private void Awake()
    {
        bullet = new Bullet("Rifle", "RifleBullet");
    }

    protected override void Action()
    {
        if (Input.GetMouseButton(0) && readyToFire && bullet.GetCurrentBullet() > 0)
        {
            FireBullet();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }
    }

    protected override void InitAmmo()
    {
        readyToFire = true;
        bullet.SetCurrentBullet(maxAmmo);
    }

    protected override IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        readyToFire = true;
        if (bullet.GetSpareBullet() > 0)
        {
            int difBullet = maxAmmo - bullet.GetCurrentBullet(); ;
            if (difBullet < 100)
            {
                bullet.SetCurrentBullet(bullet.GetCurrentBullet() + difBullet);
                bullet.SetSpareBullet(bullet.GetSpareBullet() - difBullet);
            }
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

        bullet.SetCurrentBullet(bullet.GetCurrentBullet() - 1);

        MagazineUI.frontMagazine = bullet.GetCurrentBullet();
        MagazineUI.backMagazine = bullet.GetSpareBullet();

        // Implement throw cooldown
        Invoke(nameof(ReadyToFire), fireCooldown);
    }

    private void ReadyToFire()
    {
        readyToFire = true;
    }

}
