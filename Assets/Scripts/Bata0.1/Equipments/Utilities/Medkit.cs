using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Equipment
{
    private int medkitNum;
    private int currentMedkitNum;
    private bool isUsed = false;
    // Start is called before the first frame update
    protected override void Action()
    {
        currentMedkitNum = 0;
    }

    protected override void InitAmmo()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Reload()
    {
        throw new System.NotImplementedException();
    }
}
