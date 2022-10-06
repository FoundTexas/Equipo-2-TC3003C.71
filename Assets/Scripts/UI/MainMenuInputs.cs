using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuInputs : MonoBehaviour
{
    public InputAction quitGame;
    public GameObject escbox;
    public Image esc;
    public float val;
    bool quit = false;

    private void OnEnable()
    {
        quitGame.Enable();
    }
    private void OnDisable()
    {
        quitGame.Disable();
    }
    void Update()
    {
        if (quitGame.IsPressed())
        {
            escbox.SetActive(true);
            val += Time.deltaTime;
        }
        else if (!quitGame.IsPressed())
        {
            escbox.SetActive(false);
            val = 0;
        }

        val = Mathf.Clamp(val, 0, 1.5f);
        esc.fillAmount = val / 1.5f;

        if(val >= 1.5f)
        {
            QUIT();
        }
    }
    private void QUIT()
    {
        if (!quit)
        {
            quit = true;
            Debug.Log("QUIT");
            //Application.Quit();
        }
    }
}
