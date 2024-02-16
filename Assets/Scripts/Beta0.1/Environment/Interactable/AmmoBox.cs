using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Interactable
{
    protected override void Interact()
    {
        if (FindObjectOfType<Heavy>() != null)
        {
            Heavy heavy = GameObject.Find("Heavy").GetComponent<Heavy>();
            heavy.FillAmmo();
            //FillText();
        }
        if (FindObjectOfType<Rifle>() != null)
        {
            Rifle rifle = GameObject.Find("Rifle").GetComponent<Rifle>();
            rifle.FillAmmo();
        }
    }
    /*
    private IEnumerable FillText()
    {
        Debug.Log("Filling Ammo");
        yield return new WaitForSeconds(3);
        Debug.Log("Ammo Filled");
    }
    */
}
