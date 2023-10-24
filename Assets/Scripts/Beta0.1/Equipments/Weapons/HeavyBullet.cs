using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : BulletAddon
{
    private float bulletDamage = 10f;
    protected override float DamageToSet()
    {
        Bullet bullet = new Bullet(bulletDamage);
        return bullet.GetBulletDamage();
    }
}
