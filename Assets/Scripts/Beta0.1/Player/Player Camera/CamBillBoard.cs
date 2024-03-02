using UnityEngine;

public class CamBillBoard : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private void Start()
    {
        cam = GameObject.Find("Camera").GetComponent<Transform>();
    }

    void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
