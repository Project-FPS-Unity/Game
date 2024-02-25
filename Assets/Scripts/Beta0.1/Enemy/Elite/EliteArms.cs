using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteArms : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth.health.TakeDamage(20);
        }
    }
}
