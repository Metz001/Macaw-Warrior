using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Ammo Settings")]
    public TextMeshProUGUI ammoText;
    public int ammoMax;
    [SerializeField]
    double reloadTime;
    [SerializeField]
    int ammoCount;

    double startRelaodTime;
    
    bool isReloading = false;
<<<<<<< Updated upstream
=======
    public float timeToCharge;

    [Header("Ammo Shock Settings")]
    public GameObject[] shockBatteryCount = new GameObject[4]; //bater�a, la habilidad especial 
    public bool shockMode;
    public int shockFusionCell = 1;
    private int shockMaxBattery = 4; //m�ximo de arma choque electrico
    public int shockCount; //conteo de shock 
      
    [Header("Sound and Visual")]
    public GameObject flashEffect;
    public GameObject shockFlashEffect;

  
    void Awake()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Start()
    {
        ammoCount = ammoMax;
        ammoText.text = ammoMax.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (ammoCount >0)
            {
                //Intancia balas
                ammoCount--;
                ammoText.text = ammoCount.ToString();
            }
            else
            {
                //Sonido de falta de balas
                //Llamada de Ui Manager ("No tienes balas")
            }
           
        }

        //recarga
        if (Input.GetKeyDown(KeyCode.R) && isReloading==false)
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

    }
}
