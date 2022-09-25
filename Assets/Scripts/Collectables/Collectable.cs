using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[RequireComponent(typeof (MeshRenderer))]
public class Collectable : MonoBehaviour
{
    bool isCollected;

    public Material mat, mat2;

    MeshRenderer render;

    private void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        render.material = isCollected ? mat2 : mat;
    }

    public void SetCollected(bool isCollected)
    {
        this.isCollected = isCollected;
    }

    public bool GetCollected() { return isCollected; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isCollected = true;
            FindObjectOfType<SceneManagement>().SaveValue(this);
            gameObject.SetActive(false);
        }
    }
}
