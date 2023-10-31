using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    [Header("Ammo Settings")]
    public TextMeshProUGUI ammoText;    //Texto del arma en UI
    public int magazineAmmo =1;
    public  int ammoMax; //Máxima munición para esta arma   
    public int ammoCount; //conteo de munición
    bool isReloading = false;
    public float timeToCharge;

    [Header("Ammo Shock Settings")]
    public GameObject[] shockBatteryCount = new GameObject[4]; //batería, la habilidad especial 
    public bool shockMode;
    public int shockFusionCell = 1;
    private int shockMaxBattery = 4; //máximo de arma choque electrico
    public int shockCount; //conteo de shock 
      
    [Header("Sound and Visual")]
    public GameObject flashEffect;
    public GameObject shockFlashEffect;

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

       //recarga balas
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            isReloading = true;
            //startRelaodTime = Time.time;
            StartCoroutine(Reload());
        }
          
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5);    
    }

   

    private bool TryShot()
    {
        //Debug.Log("Try Shot");
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

    
    IEnumerator Reload()
    {
        //Debug.Log("hay " + magazineAmmo + "magazine y "+ shockFusionCell + " Celulas");
        if (shockMode == false && magazineAmmo > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(timeToCharge);
            ammoCount = ammoMax;
            ammoText.text = ammoCount.ToString();
            magazineAmmo--;
            isReloading = false;
            Debug.Log("Arma cargada");
            Debug.Log("hay " + magazineAmmo + "magazine y " + shockFusionCell + " Celulas");
        }
        else if (shockMode == true && shockFusionCell > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(timeToCharge);
            shockCount = shockMaxBattery;
            BatteryUi(shockMaxBattery);
            shockFusionCell--;
            isReloading = false;
            Debug.Log("Arma cargada");
            Debug.Log("hay " + magazineAmmo + "magazine y " + shockFusionCell + " Celulas");
        }
        else
        {
            Debug.Log("No Mgazine / FusionCell");
            isReloading = false;
        }
       

        
    }
}
