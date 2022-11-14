using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerW : MonoBehaviour
{
    private bool isSpawnable;

    private void Start()
    {
        isSpawnable = true;
        if (Physics.CheckSphere(transform.position, 1))
        {
            isSpawnable = false;
        }
    }

    public bool GetSpawnBool()
    {
        return isSpawnable;
    }
}
