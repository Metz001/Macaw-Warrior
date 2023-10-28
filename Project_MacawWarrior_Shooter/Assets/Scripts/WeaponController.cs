using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{


    [Header("Ammo Settings")]
    public TextMeshProUGUI ammoText;    //Texto del arma en UI
    public int ammoMax; //Máxima munición para esta arma
    private int shockMaxBattery = 4; //máximo de arma choque electrico
    public GameObject[] shockBatteryCount = new GameObject[4]; //batería, la habilidad especial 
    [SerializeField]
    double reloadTime; //Tiempo de recarga y cambio de modo
    [SerializeField]
    int ammoCount; //conteo de munición
    int shockCount; //conteo de shock
    bool shockMode;


    double startRelaodTime; //Alamacena el momento en el que se hizo la recarga
    bool isReloading = false;


    [Header("Shock")]
    public float timeToCharge;


    
    

    // Start is called before the first frame update
    void Start()
    {     
        ammoCount = ammoMax;
        ammoText.text = ammoMax.ToString();
        
        shockMode = false;
        shockCount = shockMaxBattery;
        batteryUi(shockCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Cambio a shock
        if (Input.GetButtonDown("Fire2"))
        {                    
            if (shockMode == false)
                shockMode = true;            
            else
                shockMode = false;

            Debug.Log("Cambio de modo, shock " + shockMode);
        }
      

        //Disparo
        if (Input.GetButtonDown("Fire1") && isReloading == false)
        {
            if (ammoCount > 0 && shockMode == false)
            {
                //Intancia balas
                ammoCount--;
                ammoText.text = ammoCount.ToString();
            }
            else if (ammoCount == 0 && shockMode == false)
            {
                //Sonido de falta de balas
                //Llamada de Ui Manager ("No tienes balas")
                Debug.Log("No hay balas");
            }

            if(shockCount > 0 && shockMode == true)
            {
                shockCount--;
                batteryUi(shockCount);
            }
            else if(shockCount == 0 && shockMode == true)
            {
                //Sonido de falta de balas
                //Llamada de Ui Manager ("No tienes balas")
                Debug.Log("No hay batería");
            }

        }

        //recarga balas
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
               
    }
    void batteryUi(int x)
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
    IEnumerator ShockCharger()
    {
        yield return new WaitForSeconds(timeToCharge);
        shockCount= shockCount +1;    
        batteryUi(shockCount);
      
    }*/
}
