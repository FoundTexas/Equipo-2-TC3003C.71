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
        Ray ray = new Ray(transform.position - new Vector3(5, 0, 0), transform.right);
        Ray ray_ = new Ray(transform.position - new Vector3(0, 0, 5), transform.forward);
        RaycastHit hitData;
        RaycastHit hitData_;
        Debug.DrawRay(transform.position, transform.right * 10, Color.red);
        if (Physics.Raycast(ray, out hitData, 10, layerMask) || Physics.Raycast(ray_, out hitData_, 10, layerMask))
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
