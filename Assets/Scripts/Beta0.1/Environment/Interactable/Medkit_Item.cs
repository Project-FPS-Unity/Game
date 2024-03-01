using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Item : Interactable
{
    protected override void Interact()
    {
        PickUpMedkit();
    }

    private void PickUpMedkit()
    {
        
        if (Medkit.currentMedkit >= 1)
        {
            return;
        }
        promptMessage = "You got one Medkit";
        Medkit.currentMedkit += 1;
        Destroy(gameObject);
    } 
}
