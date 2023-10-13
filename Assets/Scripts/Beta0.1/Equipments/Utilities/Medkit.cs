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
                Debug.Log("Medkit is used");
                //Destroy(gameObject);
                currentMedkit--;
                isUsedMedkit = true;
                gameObject.SetActive(false);
            }
        }
        Debug.Log("IsUseMedKit = " + IsUsedMedkit());
    }

    public bool IsUsedMedkit()
    {
        return isUsedMedkit;
    }

    public void SetIsUsedMedkitToFalse()
    {
        isUsedMedkit = false;
        gameObject.SetActive(true);
        currentMedkit = maxMedkit;
    }
}
