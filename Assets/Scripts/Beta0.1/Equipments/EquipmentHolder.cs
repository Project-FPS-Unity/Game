using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    [Header("Equip")]
    private int currentEquipment = 0;
    [Header("Sway Settings")]
    private float smooth = 8f;
    private float swayMultiplier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        SelectedEquipment(currentEquipment);
    }

    // Update is called once per frame
    void Update()
    {
        int previousEquipment = currentEquipment;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentEquipment >= transform.childCount - 1) currentEquipment = 0;
            else currentEquipment++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentEquipment <= 0) currentEquipment = transform.childCount - 1;
            else currentEquipment--;
        }
        if (previousEquipment != currentEquipment) SelectedEquipment(currentEquipment);

        Sway();
    }

    private void SelectedEquipment(int selected)
    {
        int i = 0;
        foreach (Transform equipment in transform)
        {
            if (i == selected) equipment.gameObject.SetActive(true);
            else equipment.gameObject.SetActive(false);
            i++;
        }
    }

    private void Sway()
    {
        //Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;
        //Calculate rotation
        Quaternion rotateX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotateY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = rotateX * rotateY;
        //Sway equipments
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
