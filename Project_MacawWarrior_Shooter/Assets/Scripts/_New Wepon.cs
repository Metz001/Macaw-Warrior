using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _NewWeapon : MonoBehaviour
{
    public bool activeWeapon;
    [Header("References")]
    public Transform weaponMuzzle;

    [Header("General")]
    public LayerMask hittableLayer;
    public GameObject bulletHolePrefab;

    [Header("Shoot Parameter")]
    public float fireRange = 200; //distacia que recorre la bala
    public float recoilForce = 4f; //retroceso
    public Transform cameraPlayerTransform;
    public float fireRate = 0.6f;
    private float lastTimeShoot = Mathf.NegativeInfinity;

    [Header("Ammo Settings")]
    public int magazineAmmo = 1;
    public int ammoMax; //Máxima munición para esta arma   
    public int ammoCount; //conteo de munición
    bool isReloading = false;
    public float timeToCharge;

    [Header("Sound and Visual")]
    public GameObject flashEffect;
  
    void Awake()
    {
        activeWeapon = false;
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ammo Start
        ammoCount = ammoMax;
          
    }

    // Update is called once per frame
    void Update()
    {
               //Disparo
        if (Input.GetButtonDown("Fire1") && isReloading == false)
        {

            TryShot(); //Verifica si es posible disparar
        }

        //recarga balas
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            isReloading = true;
            StartCoroutine(Reload());
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5);
    }



    private bool TryShot()
    {
        
           //Ammo Fire
        if (lastTimeShoot + fireRate < Time.time)
        {
            if (ammoCount > 0)
            {
                HandleShot();
                ammoCount--;         
                return true;
            }
            else if (ammoCount <= 0)
            {
                //Sonido de falta de balas
                //Llamada de Ui Manager ("No tienes balas")
                Debug.Log("No hay balas");
            }


        }

        return false;
    }

    private void HandleShot()
    {       

            FlashShoot();
            AddRecoil(recoilForce);
            RaycastHit hit;
            if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange, hittableLayer))
            {
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.0001f, Quaternion.LookRotation(hit.normal));
                if (hit.transform.gameObject.CompareTag("EnergyCell"))
                {
                    try
                    {
                        hit.transform.gameObject.GetComponent<EnergyCell>().Activate();
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                }
                Destroy(bulletHoleClone, 4f);
            }
            Debug.Log("Ammo Shoot");
        



        lastTimeShoot = Time.time;

    }
  

    private void FlashShoot( )
    {
        GameObject FlashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform);
        FlashClone.transform.rotation = weaponMuzzle.rotation;
        Destroy(FlashClone, 1f);

    }

    private void AddRecoil(float recoilForce_)
    {
        transform.Rotate(-recoilForce_, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce_ / 50f);

    }
  


    IEnumerator Reload()
    {
        //Debug.Log("hay " + magazineAmmo + "magazine y "+ shockFusionCell + " Celulas");
        if (magazineAmmo > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(timeToCharge);
            ammoCount = ammoMax;         
            magazineAmmo--;
            isReloading = false;
            Debug.Log("Arma cargada");
            Debug.Log("hay " + magazineAmmo + "magazine");
        }     
        else
        {
            Debug.Log("No Mgazine / FusionCell");
            isReloading = false;
        }



    }
}
