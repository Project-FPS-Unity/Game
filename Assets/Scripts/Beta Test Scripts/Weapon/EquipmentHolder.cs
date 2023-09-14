using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    public int CurrentEquipment = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectedEquipment(CurrentEquipment);
    }

    // Update is called once per frame
    void Update()
    {
        int previousEquipment = CurrentEquipment;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (CurrentEquipment >= transform.childCount - 1) CurrentEquipment = 0;
            else CurrentEquipment++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (CurrentEquipment <= 0) CurrentEquipment = transform.childCount - 1;
            else CurrentEquipment--;
        }
        if (previousEquipment != CurrentEquipment) SelectedEquipment(CurrentEquipment);
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
}
