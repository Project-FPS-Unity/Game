using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Equipment
{
    [Header("Reference")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject objectToFire;
    [SerializeField] private MagazineUI magazineUI;

    [Header("Settings")]
    [SerializeField] private float fireCooldown;
    private Bullet bullet;
    private float reloadTime = 1f;
    private bool canReload = true;

    [Header("Throwing")]
    [SerializeField] private float bulletForce;
    [SerializeField] private float bulletUpwardForce;
    private bool readyToFire;

    private void Awake()
    {
        bullet = SetAmmoCapacity();
    }

    protected override void Action()
    {
        GetKeyToAction();
    }

    // this method call Start()
    protected override void InitValue()
    {
        readyToFire = true;
        bullet.SetCurrentBullet(bullet.GetMaxAmmo());
    }

    protected override IEnumerator Reload()
    {
        if (bullet.GetSpareBullet() > 0 && canReload == true)
        {
            canReload = false;
            int difBullet = bullet.GetMaxAmmo() - bullet.GetCurrentBullet(); ;
            if (difBullet < 100)
            {
                bullet.SetCurrentBullet(bullet.GetCurrentBullet() + difBullet);
                bullet.SetSpareBullet(bullet.GetSpareBullet() - difBullet);
            }
        }
        yield return new WaitForSeconds(reloadTime);
        canReload = true;
        readyToFire = true;
    }

    private void GetKeyToAction()
    {
        // hold left mouse
        if (Input.GetMouseButton(0) && readyToFire && bullet.GetCurrentBullet() > 0)
        {
            FireBullet();
        }
        // reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
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

    protected abstract Bullet SetAmmoCapacity();

    protected abstract void FireAnimation();
    protected abstract void ReloadAnimation();
}
