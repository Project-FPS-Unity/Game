using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAddon : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    private float damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetBulletDamage(DamageToSet());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
        {
            return;
        }
        else
        {
            targetHit = true;
        }

        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private void SetBulletDamage(float damageToSet)
    {
        damage = damageToSet;
    }
    protected abstract float DamageToSet();
}
