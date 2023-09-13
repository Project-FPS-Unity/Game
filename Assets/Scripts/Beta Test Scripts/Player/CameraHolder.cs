using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform CameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = CameraPosition.position;
        transform.rotation = CameraPosition.rotation;
    }
}
