using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionCell : MonoBehaviour
{
         
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // other.GetComponent<PlayerWeaponController>().WeaponSlots[0].shockFusionCell++;
            Debug.Log("Recordatorio Cambia el nombre");
            Destroy(gameObject);
        }
    }
}
