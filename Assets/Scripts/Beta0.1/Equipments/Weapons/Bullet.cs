using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    private string weaponName;
    private string bulletType;

    // bullet damage
    private float bulletDamage;
    private float rifleDamage = 3f;
    private float heavyRifleDamage = 10f;

    // current bullet
    private int currentBullet;
    private int rifleMagazineCapacity = 100;
    private int heavyMagazineCapacity = 30;

    // remain bullet
    private int spareBullet;
    private int rifleSpareBullet = 200;
    private int heavySpareBullet = 60;

    public Bullet(string weaponName, string bulletType)
    {
        this.weaponName = weaponName;
        this.bulletType = bulletType;
        MatchWeaponAndBullet();
    }

    private void MatchWeaponAndBullet()
    {
        if (weaponName == "Rifle" && bulletType == "RifleBullet")
        {
            bulletDamage = rifleDamage;
            currentBullet = rifleMagazineCapacity;
            spareBullet = rifleSpareBullet;
        }
        else if (weaponName == "Heavy" && bulletType == "HeavyBullet")
        {
            bulletDamage = heavyRifleDamage;
            currentBullet = heavyMagazineCapacity;
            spareBullet = heavySpareBullet;
        }
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
