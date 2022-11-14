using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerW : MonoBehaviour
{
    public LayerMask layerMask;
    private bool isSpawnable;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Obstacle");
        isSpawnable = true;
        if (Physics.CheckSphere(transform.position, 1))
        {
            isSpawnable = false;
        }
        
    }
    private void Update()
    {
        Debug.DrawRay(transform.position - new Vector3(0, 0, 5), transform.forward * 10, Color.red);
    }

    public bool GetSpawnBool()
    {
        return isSpawnable;
    }
}
