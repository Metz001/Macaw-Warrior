using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour, IInteractable
{
    [Header("References")]
    public GameObject door;
    [Header("General")]
    public bool activate = false;
    float speed;
    public bool destruction = false;

    private void Update()
    {
        if (activate)
        door.transform.Translate(transform.up * speed * Time.deltaTime);

        if(door.transform.position.y >= 10)
            activate = false;
    }
    public void Activate()
    {
        //if destruci�n = false
            //Intancias prefab humo
            //SetActive Prefab Energy Cell da�ado
            //SetActuve false prefab Energy Cell normal
            //Soltar prefab de munnici�n energ�a
      
       activate = true;
;   }

    public void Interact()
    {
        activate = true;
    }
}
