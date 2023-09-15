using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform camPosition;

    void Update()
    {
        SetCamPosition();
    }

    private void SetCamPosition()
    {
        transform.position = camPosition.position;
        transform.rotation = camPosition.rotation;
    }
}
