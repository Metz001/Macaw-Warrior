using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public List<_NewWeapon> startingWeapons = new List<_NewWeapon>();
    //Recordar cambiar nombre a WeaponController
    public Transform weaponParentSocket;
    public Transform defaultWeaponPosition;
    public Transform aimingPosition;

    public int activeWeaponIndex { get; private set; }

    private _NewWeapon[] weaponSlots = new _NewWeapon[5];

<<<<<<< Updated upstream
=======
    public _NewWeapon[] WeaponSlots { get => weaponSlots; private set => weaponSlots = value; }


>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        activeWeaponIndex = -1;

        foreach (_NewWeapon startingWeapon in startingWeapons)
        {
            AddWeapon(startingWeapon);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
     
    }
    private void SwitchWeapon(int p_weaponIndex)
    {
             
        if (p_weaponIndex != activeWeaponIndex)
        {
            weaponSlots[0].gameObject.SetActive(false);   
            weaponSlots[1].gameObject.SetActive(false);   
            weaponSlots[2].gameObject.SetActive(false);   
            weaponSlots[p_weaponIndex].gameObject.SetActive(true);
            activeWeaponIndex = p_weaponIndex;
        }
        if (p_weaponIndex != activeWeaponIndex && p_weaponIndex >= 0)
        {
            weaponSlots[p_weaponIndex].gameObject.SetActive(true);
            activeWeaponIndex = p_weaponIndex;
        }
    }

    private void AddWeapon(_NewWeapon p_weaponController)
    {
        weaponParentSocket.position = defaultWeaponPosition.position;

        //añadir arma al jugador
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                _NewWeapon weaponClone = Instantiate(p_weaponController, weaponParentSocket);
                weaponClone.gameObject.SetActive(false);

                weaponSlots[i] = weaponClone;
                return;
            }
        }
    }
}
