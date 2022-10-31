using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PassageCameraLock : MonoBehaviour
{
    public CinemachineFreeLook cmFreeCam;

    public Camera mainCamera;
    public Camera passageCamera;

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            cmFreeCam.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(false);
            passageCamera.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            cmFreeCam.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(true);
            passageCamera.gameObject.SetActive(false);
        }
    }
}
