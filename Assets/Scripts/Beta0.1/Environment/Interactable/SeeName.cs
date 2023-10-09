using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeName : Interactable
{
    protected override void Interact()
    {
        Debug.Log("Interact with " + gameObject.name);
        Destroy(gameObject);
    }
}
