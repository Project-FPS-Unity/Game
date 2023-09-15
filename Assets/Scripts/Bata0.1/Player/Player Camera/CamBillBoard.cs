using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBillBoard : MonoBehaviour
{
    public Transform cam;

    void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
