using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform FPSCam;
    void Update()
    {
        transform.LookAt(transform.position + FPSCam.forward);
    }
}
