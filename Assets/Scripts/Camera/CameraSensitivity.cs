using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    //Recieve Cinemachine camera of FreeLook type along with desired sentivity
    public CinemachineFreeLook camera;
    public float xSensitivity;
    public float ySensitivity;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Function that recieves floats for X and Y sensitivity
    //along with the Cinemachine camera
    //Modifies the camera's sensitivity using the float values
    public void SetSensitivity(float sens)
    {
        //Set desired sensitivity
        camera.m_XAxis.m_MaxSpeed += sens*50 + 50;
        camera.m_YAxis.m_MaxSpeed += sens;
    }

}
