using GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace PlanetCrashUI
{
    public class PauseMenu : SettingsMenu
    {
        public InputAction pauseInput;
        [Header("Menu Attributes")]

        [Tooltip("Reference to the settingsMenu GameObject")]
        public GameObject settingsMenu;
        [Tooltip("Reference to the quitMenu GameObject")]
        public GameObject quitMenu;
        [Tooltip("Reference to the confirmMenu GameObject")]
        public GameObject confirmMenu;
        [Tooltip("Reference to the miniMap GameObject")]
        public GameObject miniMap;
        public GameObject background;

        SceneLoader sceneLoader;
        public GameObject player;
        public Move playerMove;
        public Camera mainCamera;
        public Camera pauseCamera;
        public EventSystemUpdater updater;
        public GameObject resumeButton;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void OnEnable()
        {
            pauseInput.Enable();
            pauseInput.performed += PuseInput;
        }
        private void OnDisable()
        {
            pauseInput.Disable();
        }
        void Start()
        {
            brightness.TryGetSettings(out exposure);
            sceneLoader = FindObjectOfType<SceneLoader>();
            AudioListener.volume = 0.5f;
        }

        // ----------------------------------------------------------------------------------------------- Public Methods
        void PuseInput(InputAction.CallbackContext context){
            if(isPaused)
                    Resume();
                else
                    Pause();
        }

        /// <summary>
        /// Method that handels how the game is paused.
        /// </summary>
        void Pause()
        {
            player = GameManager.GetLocalPlayer();
            playerMove = player.GetComponent<Move>();
            mainCamera = Camera.main;
            updater.UpdateSelected(resumeButton);
            pauseCamera.gameObject.SetActive(true);
            pauseCamera.gameObject.tag = "MainCamera";
            pauseCamera.transform.position = player.transform.position;
            background.transform.position = pauseCamera.transform.position + pauseCamera.transform.forward * 3;
            background.SetActive(true);
            pauseMenu.SetActive(true);
            miniMap.SetActive(false);
            playerMove.canMove = false;
            playerMove.StopMove();
            isPaused = true;

            FindObjectOfType<EventSystemUpdater>().OnPause(true);
        }
        /// <summary>
        /// Method that handels the resume of the game.
        /// </summary>
        public void Resume()
        {
            pauseCamera.gameObject.SetActive(false);
            pauseCamera.gameObject.tag = "Untagged";
            playerMove.canMove = true;
            mainCamera.gameObject.SetActive(true);
            pauseCamera.gameObject.SetActive(false);
            background.SetActive(false);
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
            quitMenu.SetActive(false);
            confirmMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            miniMap.SetActive(true);
            FindObjectOfType<EventSystemUpdater>().OnPause(false);
        }
        /// <summary>
        /// Method that loads the Menu from the game.
        /// </summary>
        public void LoadMenu()
        {
            ConnectToServer.DisconectFromEvereywhere();
            Time.timeScale = 1f;
            sceneLoader.LoadByIndex(0);
        }
    }
}
