using GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : SettingsMenu
    {
        [Header("Menu Attributes")]

        [Tooltip("Reference to the settingsMenu GameObject")]
        public GameObject settingsMenu;
        [Tooltip("Reference to the miniMap GameObject")]
        public GameObject miniMap;

        SceneLoader sceneLoader;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Start()
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
            AudioListener.volume = 0.5f;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                if (isPaused)
                    Resume();
                else
                    Pause();


        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Method that handels how the game is paused.
        /// </summary>
        void Pause()
        {
            pauseMenu.SetActive(true);
            miniMap.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;
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
