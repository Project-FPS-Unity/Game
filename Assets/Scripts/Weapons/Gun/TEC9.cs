using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEC9 : WeaponAbstract
{
    protected override void Shoot()
    {
        flash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Dummy dummy = hit.transform.GetComponent<Dummy>();
            if (dummy != null)
            {
                dummy.TakeDamge(damage);
            }
        }
    }
}
