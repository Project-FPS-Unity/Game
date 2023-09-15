using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Equipment
{
    [Header("Camara")]
    [SerializeField] private Camera FPSCam;
    [Header("Gun Details")]
    private float Damage = 20f;
    private float Range = 100f;
    private int MaxAmmo = 50;
    private int CurrentAmmo;
    private int MaxMagazine = 5;
    private int CurrentMagazine;
    private float ReloadTime = 1f;
    private bool IsReloading = false;
    [Header("VFX")]
    [SerializeField] public ParticleSystem MuzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = MaxAmmo;
    }

    void OnEnable()
    {
        IsReloading = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (IsReloading) return;
        //Action();
    }

    public override void Action()
    {
        //if (CurrentAmmo <= 0)
        //{
        //    StartCoroutine(Reload());
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(Reload());
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Shoot();
        //}
        Debug.Log("Action!!!");
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
            //    Target target = hit.transform.GetComponent<Target>();
            //    if (target != null)
            //    {
            //        target.TakeDamage(Damage);
            //    }
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
