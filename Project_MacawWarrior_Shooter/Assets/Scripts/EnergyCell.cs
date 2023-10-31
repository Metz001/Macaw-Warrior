using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCell : MonoBehaviour
{
    public GameObject door;
    public bool activate = false;
    [SerializeField]
    float speed;

    private void Update()
    {
        if (activate)
        door.transform.Translate(transform.up * speed * Time.deltaTime);

        if(door.transform.position.y >= 10)
            activate = false;
    }
    public void Action()
    {
       activate = true;
;   }

}
