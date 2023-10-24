using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Gun
{
    [SerializeField] private ParticleSystem muzzleFlash;
    private int maxAmmo = 30;
    private int spareBullet = 60;
    /*
    protected override IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReload", true);
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        animator.SetBool("isReload", false);
        isReloading = false;
    }
    */
    protected override Bullet SetAmmoCapacity()
    {
        return new Bullet(maxAmmo, spareBullet);
    }

    protected override void FireAnimation()
    {
        throw new System.NotImplementedException();
    }

    protected override void ReloadAnimation()
    {
        throw new System.NotImplementedException();
    }
}
