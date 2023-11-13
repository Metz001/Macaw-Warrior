using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType
{
    Gun,
    Rifle,
    Destripadora,
}
public class weapo_Pick : MonoBehaviour
{
   
    public WeaponType thisWaepon = WeaponType.Gun;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && thisWaepon == WeaponType.Gun)
        {
            other.GetComponent<PlayerWeaponController>().WeaponSlots[0].activeWeapon = true;
            Destroy(gameObject);
        }  
        if (other.gameObject.CompareTag("Player") && thisWaepon == WeaponType.Gun)
        {
            other.GetComponent<PlayerWeaponController>().WeaponSlots[1].activeWeapon = true;
            Destroy(gameObject);
        } 
        if (other.gameObject.CompareTag("Player") && thisWaepon == WeaponType.Gun)
        {
            other.GetComponent<PlayerWeaponController>().WeaponSlots[2].activeWeapon = true;
            Destroy(gameObject);
        }
    }
    
}
