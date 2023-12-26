using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Equipment
{
    [Header("Reference")]
    [SerializeField] private Transform cam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject objectToThrow;

    [Header("Settings")]
    [SerializeField] private int totalThrows;
    [SerializeField] private float throwCooldown;

    [Header("Throwing")]
    [SerializeField] private float delay = 3f;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwUpwardForce;
    private bool readyToThrow;

    protected override void Action()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    protected override void InitValue()
    {
        readyToThrow = true;
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }

    private void Throw()
    {
        readyToThrow = false;

        // Insatantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // Get Rigidbody componenet
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Calculate direction
        Vector3 forceDirection = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // Add Force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // Implement throw cooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
