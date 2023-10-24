using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : BulletAddon
{
    private float bulletDamage = 3f;
    protected override float DamageToSet()
    {
        Bullet bullet = new Bullet(bulletDamage);
        return bullet.GetBulletDamage();
    }
}
