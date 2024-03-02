using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteArms : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.health.TakeDamage(20);
        }
    }
}
