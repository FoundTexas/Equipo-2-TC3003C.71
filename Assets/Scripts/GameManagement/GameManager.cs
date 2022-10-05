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
    public Vector3 CheckPoint;

    public List<Vector3> initialPositions = new List<Vector3>();

    void Start()
    {
        if (!inst)
        {
            inst = this;
        }
        else if (inst)
        {
            if (inst != this)
            {
                Destroy(this.gameObject);
            }
        }

        saved = true;
        DontDestroyOnLoad(this);
    }

    public static Vector3 FirstPos(int i)
    {
        Debug.Log(inst.initialPositions.Count);
        Debug.Log(inst.initialPositions[i]);
        inst.CheckPoint = inst.initialPositions[i];
        return inst.CheckPoint;
    }

    public static Vector3 getCheckpoint()
    {
        Debug.Log(inst.initialPositions.Count);
        if (inst.CheckPoint == Vector3.zero)
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            return FirstPos(SceneManager.GetActiveScene().buildIndex);
        }

        return inst.CheckPoint;
    }

    public static void setCheckPoint(Vector3 newPos)
    {
        inst.CheckPoint = newPos;
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

                Debug.Log("Saving: " + );
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
