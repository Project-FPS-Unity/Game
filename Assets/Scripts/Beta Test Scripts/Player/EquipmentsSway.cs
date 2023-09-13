using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentsSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float Smooth = 8f;
    [SerializeField] private float SwayMultiplier = 2f;

    // Update is called once per frame
    void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * SwayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * SwayMultiplier;
        //Calculate rotation
        Quaternion rotateX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotateY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = rotateX * rotateY;
        //Sway equipments
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Smooth * Time.deltaTime);
    }
}
