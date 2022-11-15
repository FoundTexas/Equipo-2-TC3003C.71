using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlanetCrashUI;
using UnityEngine.InputSystem;

public class LevelSelectUI : MonoBehaviour
{
    public InputAction pauseInput;
    public GameObject levelSelectMenu;
    public GameObject menuCamera;
    public GameObject canvas;
    public GameObject pauseMenu;
    public GameObject pauseCamera;
    private void OnEnable()
        {
            pauseInput.Enable();
            pauseInput.performed += PuseInput;
        }
        private void OnDisable()
        {
            pauseInput.Disable();
        }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && (!GameManager.isOnline || PhotonNetwork.IsMasterClient))
        {
            if(levelSelectMenu.activeInHierarchy == false)
            {
                canvas.GetComponent<PauseMenu>().Pause();
                pauseMenu.SetActive(false);
                pauseCamera.SetActive(false);
                levelSelectMenu.SetActive(true);
                menuCamera.SetActive(true);
            }                 
        }
    }

    void PuseInput(InputAction.CallbackContext context){
            if(levelSelectMenu.activeInHierarchy == true)
                    ExitMenu();
        }

    public void ExitMenu()
    {
        levelSelectMenu.SetActive(false);
        menuCamera.SetActive(false);
        canvas.GetComponent<PauseMenu>().Resume();
    }

}