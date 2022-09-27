using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class SettingsMenu : MonoBehaviour
    {

        public static bool isPaused = false;

        [Header("Settings Atributes")]

        [Tooltip("Reference to the pause menu GameObject")]
        public GameObject pauseMenu;

        new GameObject camera;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void Start()
        {
            camera = GameObject.Find("ThirdPersonCamera");
            AudioListener.volume = 0.5f;
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Method that handels the Listener volume.
        /// </summary>
        /// <param name="volume"> Amount of volume (0f to 1f). </param>
        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
        }
        /// <summary>
        /// Void in charge of handeling the game brightness.
        /// </summary>
        /// <param name="gamma"> Amount of brightness (0f to 1f). </param>
        public void SetBrightness(float gamma)
        {
            RenderSettings.ambientSkyColor = new Color(gamma, gamma, gamma, 0f);
        }
        /// <summary>
        /// Void that changes the mouse sensitivity.
        /// </summary>
        /// <param name="sensFactor"> amount to be changed. </param>
        public void SetSensitivity(float sensFactor)
        {
            if (camera != null)
            {
                var sens = camera.GetComponent<CameraSensitivity>();
                sens.SetSensitivity(sensFactor);
            }

        }
        /// <summary>
        /// Void that quits the application.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
