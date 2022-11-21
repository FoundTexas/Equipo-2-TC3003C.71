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
    public GameObject background;
    public EventSystemUpdater events;
    public GameObject firstButton;
    public GameObject resumeButton;
    public GameObject fragmentsUI, healthBar, ammoUI;
    
    private void OnEnable()
    {
        pauseInput.Enable();
        pauseInput.performed += PauseInput;
    }
    private void OnDisable()
    {
        pauseInput.Disable();
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && (!GameManager.isOnline || (PhotonNetwork.IsMasterClient && col.gameObject.GetComponent<PhotonView>().IsMine)))
        {
            if(levelSelectMenu.activeInHierarchy == false)
            {
                canvas.GetComponent<PauseMenu>().Pause();
                events.UpdateSelected(firstButton);
                pauseMenu.SetActive(false);
                pauseCamera.SetActive(false);
                background.SetActive(false);
                levelSelectMenu.SetActive(true);
                menuCamera.SetActive(true);
                fragmentsUI.SetActive(false);
                healthBar.SetActive(false);
                ammoUI.transform.localScale = Vector3.zero;
            }                 
        }
    }

    void PauseInput(InputAction.CallbackContext context){
            if(levelSelectMenu.activeInHierarchy == true)
                    ExitMenu();
        }

    public void ExitMenu()
    {
        levelSelectMenu.SetActive(false);
        menuCamera.SetActive(false);
        events.UpdateSelected(resumeButton);
        fragmentsUI.SetActive(true);
        healthBar.SetActive(true);
        ammoUI.transform.localScale = new Vector3(1f, 1f, 1f);
        canvas.GetComponent<PauseMenu>().Resume();
    }

}
