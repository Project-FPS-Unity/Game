using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Item : Interactable
{
    private Medkit medkit;
    protected override void Interact()
    {
        Debug.Log("You get one Medkit");
        medkit.SetCurrentMedkit(1);
        Destroy(gameObject);
    }
}
