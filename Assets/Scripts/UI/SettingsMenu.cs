using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenu;
    public int menuScene;
    SceneLoader sceneLoader;
    GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("ThirdPersonCamera");
        sceneLoader = FindObjectOfType<SceneLoader>();
        AudioListener.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVolume(float volume)
    {
        AudioListener.volume  = volume;
    }

    public void SetBrightness(float gamma)
    {
        RenderSettings.ambientSkyColor = new Color(gamma, gamma, gamma, 0f);
    }

    public void SetSensitivity(float sensFactor)
    {
        if(camera != null)
        {
            var sens = camera.GetComponent<CameraSensitivity>();
            sens.SetSensitivity(sensFactor);
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
