using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteArms : MonoBehaviour
{
    public bool ishit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ishit = true;
        }
    }
}
