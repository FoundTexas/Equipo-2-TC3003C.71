using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScale : MonoBehaviour
{
    public float distance;
    private float width;
    private float height;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        height = 2f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
        width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3(width / 9f, width / 9f, width / 9f);
    }
}
