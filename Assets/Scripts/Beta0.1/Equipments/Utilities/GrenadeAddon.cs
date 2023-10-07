using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAddon : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    private float damage = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

            Destroy(gameObject);
        }

        rb.isKinematic = true;

        transform.SetParent(collision.transform);
    }
}
