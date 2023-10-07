using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : BulletAddon
{
    private float rifleDamage = 2f;

    protected override float DamageToSet()
    {
        return rifleDamage;
    }
}
