using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    public GameObject miniBoss;
    public GameObject door;
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            miniBoss.SetActive(true);
            door.SetActive(true);
            door.GetComponent<Animation>().Play("BlockDoor");
        }
    }
}
