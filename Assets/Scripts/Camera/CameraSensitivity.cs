using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSensitivity : MonoBehaviour
{
    //Recieve Cinemachine camera of FreeLook type along with desired sentivity
    public CinemachineFreeLook camera;
    public float xSensitivity = 400f;
    public float ySensitivity = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetSensitivity(xSensitivity, ySensitivity, camera);
    }
    
    //Function that recieves floats for X and Y sensitivity
    //along with the Cinemachine camera
    //Modifies the camera's sensitivity using the float values
    public void SetSensitivity(float xSens, float ySens, CinemachineFreeLook camera)
    {
        //Set desired sensitivity
        camera.m_XAxis.m_MaxSpeed = xSens;
        camera.m_YAxis.m_MaxSpeed = ySens;
    }

}
