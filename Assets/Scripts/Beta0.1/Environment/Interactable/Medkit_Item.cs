using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Item : Interactable
{
    [SerializeField] private Medkit medkit;
    protected override void Interact()
    {
        Debug.Log("You get one Medkit");
        medkit.SetIsUsedMedkitToFalse();
        Destroy(gameObject);
    }
}
