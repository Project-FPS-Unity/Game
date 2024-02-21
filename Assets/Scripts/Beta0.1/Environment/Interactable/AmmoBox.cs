using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    protected override void Interact()
    {
        FillAmmo();
    }

    private void FillAmmo()
    {
        if (FindObjectOfType<Heavy>() != null)
        {
            Heavy heavy = GameObject.Find("Heavy").GetComponent<Heavy>();
            heavy.FillAmmo();
            Destroy(gameObject);
            //FillText();
        }
        if (FindObjectOfType<Rifle>() != null)
        {
            Rifle rifle = GameObject.Find("Rifle").GetComponent<Rifle>();
            rifle.FillAmmo();
            Destroy(gameObject);
        }
    }
}
