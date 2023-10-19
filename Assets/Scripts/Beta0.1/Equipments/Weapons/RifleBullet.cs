using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : BulletAddon
{
    protected override float DamageToSet()
    {
        Bullet bullet = new Bullet("Rifle", "RifleBullet");
        return bullet.GetBulletDamage();
    }
}
