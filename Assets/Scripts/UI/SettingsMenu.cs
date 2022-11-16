using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

namespace PlanetCrashUI {
    public class SettingsMenu : MonoBehaviour
    {

        public static bool isPaused = false;

        [Header("Settings Atributes")]

        [Tooltip("Reference to the pause menu GameObject")]
        public GameObject pauseMenu;
        public PostProcessProfile brightness;
        public PostProcessLayer layer;
        public AutoExposure exposure;

        new public GameObject camera;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void Start()
        {
            brightness.TryGetSettings(out exposure);
            camera = GameObject.FindWithTag("CinemachineCamera");
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
        /// <param name="value"> Amount of brightness (0.1f to 2f). </param>
        public void SetBrightness(float value)
        {
            if(exposure != null)
                exposure.keyValue.value = value;
        }
        /// <summary>
        /// Void that changes the mouse sensitivity.
        /// </summary>
        /// <param name="sensFactor"> amount to be changed. </param>
        public void SetSensitivity(float sensFactor)
        {
            camera = GameObject.FindWithTag("CinemachineCamera");
            if (camera != null)
            {
                var sens = camera.GetComponent<CameraSensitivity>();
                sens.SetSensitivity(sensFactor);
            }

        }
        /// <summary>
        /// Void that changes the mouse sensitivity.
        /// </summary>
        /// <param name="sensFactor"> amount to be changed. </param>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
