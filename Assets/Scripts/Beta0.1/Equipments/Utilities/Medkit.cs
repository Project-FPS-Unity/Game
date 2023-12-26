using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Equipment
{
    // Constraint
    private int maxMedkit = 1;
    public static int currentMedkit;
    private float healAmount = 20f;
    public static bool haveMedkit = true;

    private PlayerHealth player;

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
    private void UseMedkit()
    {
        if(currentMedkit > 0)
        {
            //gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                PlayerHealth.health.SetHealth(PlayerHealth.health.GetCurrentHealth() + healAmount);
                Debug.Log("Medkit is used");
                //Destroy(gameObject);
                currentMedkit--;
                //isUsedMedkit = true;
                gameObject.SetActive(false);
            }
        }
        //Debug.Log("IsUseMedKit = " + IsUsedMedkit());
    }

    private void InitMedkit()
    {
        currentMedkit = maxMedkit;
    }

    private void OnDisable()
    {
        if (currentMedkit == 1)
        {
            haveMedkit = true;
        }
        else if (currentMedkit == 0)
        {
            haveMedkit = false;
        }
    }

    //public bool IsUsedMedkit()
    //{
    //    return isUsedMedkit;
    //}

    //public void SetIsUsedMedkitToFalse()
    //{
    //    isUsedMedkit = false;
    //    gameObject.SetActive(true);
    //    currentMedkit = maxMedkit;
    //}
}
