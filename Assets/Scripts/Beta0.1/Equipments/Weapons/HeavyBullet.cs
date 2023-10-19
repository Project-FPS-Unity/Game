using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : BulletAddon
{
    protected override float DamageToSet()
    {
        Bullet bullet = new Bullet("Heavy", "HeavyBullet");
        return bullet.GetBulletDamage();
    }
}
