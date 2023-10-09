using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBullet : BulletAddon
{
    private float heavyDamage = 10f;

    protected override float DamageToSet()
    {
        return heavyDamage;
    }
}
