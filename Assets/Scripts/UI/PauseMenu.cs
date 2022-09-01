using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject miniMap;
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
        if(Input.GetKeyDown(KeyCode.Escape))
            if(isPaused)
                Resume();
            else
                Pause();
            
            
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        miniMap.SetActive(true);
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        miniMap.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
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
        var sens = camera.GetComponent<CameraSensitivity>();
        sens.SetSensitivity(sensFactor);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        sceneLoader.LoadByIndex(menuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
