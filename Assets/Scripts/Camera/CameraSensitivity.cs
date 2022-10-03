using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Class that manages the camera movement according to a given sensitivity.
/// </summary>
public class CameraSensitivity : MonoBehaviour
{
    [Tooltip("Recieve Cinemachine camera of FreeLook type along with desired sentivity")]
    new public CinemachineFreeLook camera;

    /// <summary>
    /// Function that recieves floats for X and Y sensitivity along with the Cinemachine camera.
    /// Modifies the camera's sensitivity using the float values.
    /// </summary>
    /// <param name="sens"> float value specifying the camera's sensitivity </param>
    public void SetSensitivity(float sens)
    {
        //Set desired sensitivity
        camera.m_XAxis.m_MaxSpeed = (sens * 50) + 50;
        camera.m_YAxis.m_MaxSpeed = sens;
    }

}
