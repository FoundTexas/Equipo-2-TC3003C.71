using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Open level select...");
            }
        }
    }
}
