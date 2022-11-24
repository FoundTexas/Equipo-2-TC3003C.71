using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HintUI : MonoBehaviour
{
    TMP_Text objective;

    void OnEnable()
    {
        objective = GetComponent<TMP_Text>();
        int idx = SceneManager.GetActiveScene().buildIndex;
        switch(idx)
        {
            case 0:
                objective.text = "Begin the game!";
                break;
            case 1:
                objective.text = "Escape the junkyard!";
                break;
            case 2:
                objective.text = "Select a destination on the terminal!";
                break;
            case 3:
                objective.text = "Find the Planetary Fragments!";
                break;
            case 4:
                objective.text = "Defeat the fake conductor!";
                break;
            default:
                objective.text = "";
                break;
        }
        
    }
}
