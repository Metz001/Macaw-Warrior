using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour, IInteractable
{
    [Header("References")]
    public GameObject door;
    public GameObject fusionCellPrefab;
    public GameObject energyCellBroke;
    public GameObject energyCell;

    [Header("General")]
    public bool activate = false;
    float speed;
    public bool destroyed = false;

    private void Update()
    {
        if (activate)
        door.transform.Translate(transform.up * speed * Time.deltaTime);

        if(door.transform.position.y >= 10)
            activate = false;
    }
    public void Activate()
    {
        //if destrución = false
        //Intancias prefab humo
        //SetActive Prefab Energy Cell dañado
        //SetActuve false prefab Energy Cell normal
        //Soltar prefab de munnición energía
        if (destroyed == false)
        {
            energyCell.SetActive(false);
            energyCellBroke.SetActive(true);
            destroyed = true;
            Drop();
        }
      
       activate = true;
;   }

    public void Interact()
    {
        activate = true;
    }

    private void Drop()
    {
        //Intanciar la célula de fusion
        GameObject newFusionCell = Instantiate(fusionCellPrefab, gameObject.transform.position + transform.up * 2, gameObject.transform.rotation);
        // obtener rigid body
        Rigidbody fusionCellRb = newFusionCell.GetComponent<Rigidbody>();
        //añadir fuerza, movimeinto
        Vector3 forceToAdd = transform.up * 3 + transform.forward * 2;

        fusionCellRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
