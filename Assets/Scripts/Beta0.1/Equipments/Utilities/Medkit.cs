using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Medkit : Equipment
{
    // Constraint
    private int maxMedkit = 1;
    public static int currentMedkit;
    private float healAmount = 20f;
    public static bool haveMedkit = true;

    private void Awake()
    {
        //var camHolder = GetComponentInParent<FPS>();
        //player = camHolder.GetComponentInChildren<PlayerScript>();
    }
    // Start is called before the first frame update
    protected override void Action()
    {
        UseMedkit();
    }
    protected override void InitValue()
    {
        InitMedkit();
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }

    // Equipment Action
    public void UseMedkit()
    {
        if(currentMedkit > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerHealth.health.SetHealth(PlayerHealth.health.GetCurrentHealth() + healAmount);
                currentMedkit--;
            }
        }
    }

    private void InitMedkit()
    {
        currentMedkit = maxMedkit;
    }
}
