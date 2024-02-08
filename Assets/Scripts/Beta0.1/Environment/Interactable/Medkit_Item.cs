using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Item : Interactable
{
    //[SerializeField] private Medkit medkit;
    protected override void Interact()
    {
        PickUpMedkit();
    }

    private void PickUpMedkit()
    {
        
        if (Medkit.currentMedkit >= 1)
        {
            Debug.Log("Medkit Full");
            return;
        }
        Debug.Log("You get one Medkit");
        promptMessage = "You got one Medkit";
        Medkit.currentMedkit += 1;
        Destroy(gameObject);
    } 
}
