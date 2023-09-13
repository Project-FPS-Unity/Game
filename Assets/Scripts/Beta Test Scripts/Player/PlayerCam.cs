using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float SenX = 400f;
    public float SenY = 400f;

    public Transform Orientation;

    float RotateX;
    float RotateY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * SenX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * SenY;

        RotateY += mouseX;
        RotateX -= mouseY;
        RotateX = Mathf.Clamp(RotateX, -90f, 90f);

        //Rotate cam
        transform.rotation = Quaternion.Euler(RotateX, RotateY, 0);
        Orientation.rotation = Quaternion.Euler(0, RotateY, 0);
    }
}
