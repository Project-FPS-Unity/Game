using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEC9 : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem flash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        flash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Dummy dummy = hit.transform.GetComponent<Dummy>();
            if (dummy != null)
            {
                dummy.TakeDamge(damage);
            }
        }
    }
}
