using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Camara")]
    public Camera FPSCam;
    [Header("Gun Details")]
    public float Damage = 20f;
    public float Range = 100f;
    public int MaxAmmo = 50;
    private int CurrentAmmo;
    public int MaxMagazine = 5;
    private int CurrentMagazine;
    public float ReloadTime = 1f;
    private bool IsReloading = false;
    [Header("VFX")]
    public ParticleSystem MuzzleFlash;

    void Start()
    {
        CurrentAmmo = MaxAmmo;
    }

    void OnEnable()
    {
        IsReloading = false;
        
    }

    void Update()
    {
        if (IsReloading) return;

        if (CurrentAmmo <= 0) {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        MuzzleFlash.Play();
        CurrentAmmo--;
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

    private IEnumerator Reload()
    {
        IsReloading = true;

        yield return new WaitForSeconds(ReloadTime);

        CurrentAmmo = MaxAmmo;
        IsReloading = false;
    }
}
