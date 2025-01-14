using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    // bullet damage
    private float bulletDamage;

    // capacity
    private int currentBullet;
    private int spareBullet;
    private int maxAmmo;

    public Bullet(float bulletDamage)
    {
        this.bulletDamage = bulletDamage;
    }

    public Bullet(int maxAmmo, int spareBullet)
    {
        this.maxAmmo = maxAmmo;
        this.spareBullet = spareBullet;
        this.currentBullet = maxAmmo;
    }

    // getter
    public float GetBulletDamage()
    {
        return bulletDamage;
    }

    public int GetCurrentBullet()
    {
        return currentBullet;
    }

    public int GetSpareBullet()
    {
        return spareBullet;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
    
    // setter
    public void SetCurrentBullet(int bulletToSet)
    {
        currentBullet = bulletToSet;
    }

    public void SetSpareBullet(int spareBulletToSet)
    {
        spareBullet = spareBulletToSet;
    }
}
