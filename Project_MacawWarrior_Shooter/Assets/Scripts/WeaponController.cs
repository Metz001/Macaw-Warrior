using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
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
    
    
    [Header("Reload")]
    public double reloadTime; //Tiempo de recarga y cambio de modo



    [Header("Ammo Settings")]
    public TextMeshProUGUI ammoText;    //Texto del arma en UI
    public  int ammoMax; //Máxima munición para esta arma   
    public int ammoCount; //conteo de munición
  

    [Header("Ammo Shock Settings")]
    public GameObject[] shockBatteryCount = new GameObject[4]; //batería, la habilidad especial 
    public bool shockMode;
    private int shockMaxBattery = 4; //máximo de arma choque electrico
    public int shockCount; //conteo de shock 
      
    [Header("Sound and Visual")]
    public GameObject flashEffect;
    public GameObject shockFlashEffect;

    double startRelaodTime; //Alamacena el momento en el que se hizo la recarga
    bool isReloading = false;


    [Header("Shock")]
    public float timeToCharge;



    void Awake()
    {

        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Ammo Start
        ammoCount = ammoMax;
        ammoText.text = ammoCount.ToString();
        //Shock Start
        shockMode = false;
        shockCount = shockMaxBattery;
        BatteryUi(shockCount);
    }

    // Update is called once per frame
    void Update()
    {      
        //Cambio a shock
        if (Input.GetButtonDown("Fire2"))
        {
            ChangeMode();        
        }

        //Disparo
        if (Input.GetButtonDown("Fire1") && isReloading == false)
        {
            
            TryShot(); //Verifica si es posible disparar
        }

      /*  //recarga balas
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            StartCoroutine(Reload());
        }*/
            
            if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
            {
                isReloading = true;
                startRelaodTime = Time.time;
                Debug.Log("Regando...");
            }
            //Tiempo de recarga
            if (startRelaodTime + reloadTime < Time.time && isReloading == true)
            {
                ammoCount = ammoMax;             
                ammoText.text = ammoCount.ToString();         
                isReloading = false;
            }

            /*
            if(shockCount < 4)
            {
                StartCoroutine("ShockCharger");
                Debug.Log("charging...");
                Debug.Log("Shock count " + shockCount);


                //if(shockCount == 4)
              //  StopCoroutine("ShockCharger");
            }*/
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5);    
    }

   

    private bool TryShot()
    {
        Debug.Log("Try Shot");
        //Shock fire
        if (shockMode == true && shockCount >0 )
        {

            Debug.Log("Shock Shoot");
            HandleShot();
            shockCount--;
            BatteryUi(shockCount);
        }
        else if (shockCount <= 0 && shockMode == true)
        {
            //Sonido de falta de balas
            //Llamada de Ui Manager ("No tienes balas")
            Debug.Log("No hay batería");
        }
        //Ammo Fire
        if (lastTimeShoot + fireRate < Time.time)
        {
            if (ammoCount > 0 && shockMode==false)
            {
                HandleShot();
                ammoCount--;
                ammoText.text = ammoCount.ToString();
                return true;
            }
            else if (ammoCount <= 0 && shockMode == false)
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
        if (shockMode == true)
        {
            Debug.Log("Shok Shoot F");
            FlashShoot(shockMode);
            AddRecoil(recoilForce / 2f);
        }

        if (shockMode == false)
        {
           FlashShoot(shockMode);
           AddRecoil(recoilForce);
           RaycastHit hit;
           if(Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange, hittableLayer))
           {
              GameObject bulletHoleClone = Instantiate(bulletHolePrefab,hit.point + hit.normal *0.0001f,Quaternion.LookRotation(hit.normal));
              Destroy(bulletHoleClone, 4f);
           }
            Debug.Log("Ammo Shoot");
        }
                        
       
           
        lastTimeShoot = Time.time;
        
    }

    private void ChangeMode()
    {
        if (shockMode == false)
        {
            shockMode = true;
        }
        else
        {
            shockMode = false;
        }
        

        Debug.Log("Cambio de modo, shock " + shockMode);
    }


    private void FlashShoot(bool shockMode)
    {
        if (shockMode)
        {
            GameObject flashClone = Instantiate(shockFlashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform);
            flashClone.transform.rotation = weaponMuzzle.rotation;
            Destroy(flashClone, 1f);
        }
        else
        {
            GameObject shockFlashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform);
            shockFlashClone.transform.rotation = weaponMuzzle.rotation;
            Destroy(shockFlashClone, 1f);
        }
    }

    private void AddRecoil(float recoilForce_)
    {
        transform.Rotate(-recoilForce_, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce_/50f);

    }

    void BatteryUi(int x)
    {
       
        switch (x)
        {
            case 0:
                shockBatteryCount[0].SetActive(false);
                shockBatteryCount[1].SetActive(false);
                shockBatteryCount[2].SetActive(false);
                shockBatteryCount[3].SetActive(false);

                break;

            case 1:
                shockBatteryCount[0].SetActive(true);
                shockBatteryCount[1].SetActive(false);
                shockBatteryCount[2].SetActive(false);
                shockBatteryCount[3].SetActive(false);
                break;
            case 2:
                shockBatteryCount[0].SetActive(true);
                shockBatteryCount[1].SetActive(true);
                shockBatteryCount[2].SetActive(false);
                shockBatteryCount[3].SetActive(false);
                break;
            case 3:
                shockBatteryCount[0].SetActive(true);
                shockBatteryCount[1].SetActive(true);
                shockBatteryCount[2].SetActive(true);
                shockBatteryCount[3].SetActive(false);
                break;
            case 4:
                shockBatteryCount[0].SetActive(true);
                shockBatteryCount[1].SetActive(true);
                shockBatteryCount[2].SetActive(true);
                shockBatteryCount[3].SetActive(true);
                break;


        }
    }

    /*
    IEnumerator Reload()
    {
        Debug.Log("Recargando...");
        if(shockMode == false)
        {
            yield return new WaitForSeconds(timeToCharge);
            ammoCount = ammoMax;
            ammoText.text = ammoCount.ToString();
            Debug.Log("Arma cargada");
        }
        else
        {
            yield return new WaitForSeconds(timeToCharge);
            shockCount = shockMaxBattery;
            BatteryUi(shockMaxBattery);
            Debug.Log("Arma cargada");
        }
        
        
      
    }*/
}
