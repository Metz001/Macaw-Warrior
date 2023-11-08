using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerWeaponController>().WeaponSlots[0].magazineAmmo++;
            Destroy(gameObject);
        }
    }
}
