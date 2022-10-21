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
    public static bool isOnline = false;
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
        inst.CheckPoint = inst.initialPositions[i];
        return inst.CheckPoint;
    }

    public static Vector3 getCheckpoint()
    {
        if (inst.CheckPoint == Vector3.zero)
        {
            return FirstPos(SceneManager.GetActiveScene().buildIndex);
        }

        return inst.CheckPoint;
    }

    public static void setCheckPoint(Vector3 newPos)
    {
        inst.CheckPoint = newPos;
    }

    public static void SetEventReference(string val)
    {
        eventRef = val;
    }
    public static void SaveGame()
    {
        saved = false;
        var saves = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        foreach (ISave save in saves)
        {
            bool isSaved = false;

            while (!isSaved)
            {
                isSaved = save.Save();
            }
        }
        if (eventRef !="")
        {
            string s = PlayerPrefs.GetString(eventRef);

            GameObject newObj = new GameObject("Name");

            tmpevent = Instantiate(newObj, Vector3.zero, Quaternion.identity).AddComponent<InGameEvent>();
            JsonUtility.FromJsonOverwrite(s, tmpevent);

            tmpevent.values.Ended = true;
            
            PlayerPrefs.SetString(eventRef, JsonUtility.ToJson(tmpevent));
            Destroy(tmpevent);
        }
        saved = true;
    }
}
