using UnityEngine;

public class CamBillBoard : MonoBehaviour
{
    [SerializeField] private Transform cam;

    void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
