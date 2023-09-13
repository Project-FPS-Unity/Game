using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Camara")]
    public Camera FPSCam;
    [Header("Gun Details")]
    public float Damage = 20f;
    public float Range = 100f;
    public ParticleSystem MuzzleFlash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        MuzzleFlash.Play();
        RaycastHit hit;
        bool isHit;
        isHit = Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, Range);
        if (isHit)
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(Damage);
            }
        }
        else
        {
            Debug.Log("Not hit");
        }
    }
}
