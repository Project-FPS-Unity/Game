using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float senX = 400f;
    public float senY = 400f;

    public Transform orientation;

    float rotateX;
    float rotateY;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        rotateY += mouseX;
        rotateX -= mouseY;
        rotateX = Mathf.Clamp(rotateX, -90f, 90f);

        //Rotate cam
        RotateCam();
    }

    private void RotateCam()
    {
        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);
    }
}
