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

    private void OnTriggerEnter(Collider collision)
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
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth player = collision.gameObject.GetComponentInParent<PlayerHealth>();
            player.TakeDamage(damage);
            // Debug.Log(collision.gameObject.GetComponentInParent<PlayerHealth>());
        }
        if (collision.gameObject.layer != 8) // Layer 8 --> Invisible Wall
        {
            Destroy(gameObject);
        }     
    }

    private void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void SetBulletDamage(float damageToSet)
    {
        damage = damageToSet;
    }
    protected abstract float DamageToSet();
}
