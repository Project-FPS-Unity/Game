using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Equipment
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float delay = 3f;
    private float countdown;
    private int maxGrenade = 2;
    private int currentGrenade;

    private bool hasExploded = false;

    protected override void Action()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    protected override void InitAmmo()
    {
        explosionEffect = GetComponent<GameObject>();
        currentGrenade = maxGrenade;
        countdown = delay;
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }

    private void Explode()
    {
        Debug.Log("BOOM!");
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
