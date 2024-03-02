using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletAddon
{
    private float bulletDamage = 4f;
    protected override float DamageToSet()
    {
        Bullet bullet = new Bullet(bulletDamage);
        return bullet.GetBulletDamage();
    }
}
