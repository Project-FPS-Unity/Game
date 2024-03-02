using UnityEngine;

public class CamBillBoard : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private Canvas canvas;
    private void Start()
    {
        //cam = GameObject.Find("Player").GetComponent<Camera>().transform;
        //canvas = gameObject.GetComponent<Canvas>();
        //canvas.worldCamera = GameObject.Find("Player").GetComponent<Camera>();
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
