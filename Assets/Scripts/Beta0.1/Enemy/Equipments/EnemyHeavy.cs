using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : EnemyGun
{
    [SerializeField] private ParticleSystem muzzleFlash;
    private int maxAmmo = 30;
    private int spareBullet = 60;

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
