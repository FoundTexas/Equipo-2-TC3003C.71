using GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseMenu : SettingsMenu
    {
        public InputAction pauseInput;
        [Header("Menu Attributes")]

        [Tooltip("Reference to the settingsMenu GameObject")]
        public GameObject settingsMenu;
        [Tooltip("Reference to the miniMap GameObject")]
        public GameObject miniMap;

        SceneLoader sceneLoader;

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
            pauseMenu.SetActive(true);
            miniMap.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;

            FindObjectOfType<EventSystemUpdater>().OnPause(true);
        }
        /// <summary>
        /// Method that handels the resume of the game.
        /// </summary>
        public void Resume()
        {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(false);
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
            Time.timeScale = 1f;
            sceneLoader.LoadByIndex(0);
        }
    }
}
