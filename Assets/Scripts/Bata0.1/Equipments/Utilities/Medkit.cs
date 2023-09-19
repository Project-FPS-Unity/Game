using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Equipment
{
    private int medkitNum = 1;
    private int currentMedkitNum;
    private float healAmount = 20f;
    private PlayerScript player;

    private void Awake()
    {
        var camHolder = GetComponentInParent<FPS>();
        player = camHolder.GetComponentInChildren<PlayerScript>();
    }
    // Start is called before the first frame update
    protected override void Action()
    {
        UseMedkit();
    }

    protected override void InitAmmo()
    {
        currentMedkitNum = medkitNum;
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }

    // Equipment Action
    private void UseMedkit()
    {
        if (Input.GetMouseButtonDown(0) && currentMedkitNum > 0)
        {
            player.UseMedkit(healAmount);
            currentMedkitNum--;
            Debug.Log("Medkit is used");
            //Destroy(gameObject);
        }
        if (currentMedkitNum <= 0)
        {
            //Debug.Log("You have no Medkit");
        }
    }
}
