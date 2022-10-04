using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameManagement;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public static string eventRef = "";
    static InGameEvent tmpevent;
    public static bool saved;
    static Vector3 CheckPoint;

    public Vector3[] initialPositions;

    void Start()
    {
        saved = true;
    }

    public static void FirstPos(int i)
    {
        CheckPoint = inst.initialPositions[i];
    }

    public static Vector3 getCheckpoint()
    {
        if(CheckPoint == Vector3.zero)
        {
            FirstPos(SceneManager.GetActiveScene().buildIndex);
        }

        return CheckPoint;
    }

    public static void setCheckPoint(Vector3 newPos)
    {
        CheckPoint = newPos;
    }

    public static void SetEventReference(string val, InGameEvent val2)
    {
        eventRef = val;
        tmpevent = val2;
    }
    public static void SaveGame()
    {
        saved = false;
        var saves = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        foreach (ISave save in saves)
        {
            bool isSaved = save.Save();
            while (!isSaved)
            {
                Debug.Log("Saving");
            }
        }
        if (tmpevent)
        {
            tmpevent.Ended = true;
            PlayerPrefs.SetString(eventRef, JsonUtility.ToJson(tmpevent));
            tmpevent = null;
        }
        saved = true;
    }
}
