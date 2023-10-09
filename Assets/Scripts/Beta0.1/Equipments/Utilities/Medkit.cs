using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Equipment
{
    private int maxMedkit = 1;
    public int currentMedkit = 1;
    private float healAmount = 20f;
    private PlayerScript player;
    private bool isUsedMedkit = false;

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
        SetIsUsedMedkitToFalse();
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
                player.UseMedkit(healAmount);
                currentMedkit--;
                Debug.Log("Medkit is used");
                //Destroy(gameObject);
            }
        }
        if (currentMedkit <= 0)
        {
            //Debug.Log("You have no Medkit");
            //Destroy(this.gameObject);
            
            SetIsUsedMedkitToTrue();
        }
        
    }

    public bool IsUsedMedkit()
    {
        return isUsedMedkit;
    }

    public void SetIsUsedMedkitToFalse()
    {
        gameObject.SetActive(true);
        isUsedMedkit = false;
        currentMedkit = maxMedkit;
    }

    public void SetIsUsedMedkitToTrue()
    {
        gameObject.SetActive(false);
        isUsedMedkit = true;
    }
}
