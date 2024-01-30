using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string promptMessage = "[E] to pickup";

    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        Debug.Log("Interact");
    }
}
