using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameManagement;
using UnityEngine.SceneManagement;
using Photon.Pun;

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

    public static int getSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static Vector3 FirstPos(int i)
    {
        inst.CheckPoint = inst.initialPositions[i];
        return inst.CheckPoint;
    }

    public static Vector3 getCheckpoint()
    {
        Vector3 pos = Vector3.zero;
        if (PhotonNetwork.IsMasterClient && isOnline || !isOnline)
        {
            pos = inst.CheckPoint;
            if (inst.CheckPoint == Vector3.zero)
            {
                pos = FirstPos(SceneManager.GetActiveScene().buildIndex);
            }
            if (isOnline && PhotonNetwork.IsMasterClient)
            {
                var hash = PhotonNetwork.CurrentRoom.CustomProperties;

                if(hash.ContainsKey("CheckPoint"))
                {
                    hash["CheckPoint"] = JsonUtility.ToJson(pos);
                }
                else if(!hash.ContainsKey("CheckPoint"))
                {
                    hash.Add("CheckPoint", JsonUtility.ToJson(pos));
                }
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }
        }
        else if (!PhotonNetwork.IsMasterClient && isOnline)
        {
            string json = PhotonNetwork.CurrentRoom.CustomProperties["CheckPoint"].ToString();
            if(json != "")
            {
                pos = JsonUtility.FromJson<Vector3>(json);
            }
            else
            {
                pos = FirstPos(SceneManager.GetActiveScene().buildIndex);
            }
        }
        return pos;
    }

    public static void setCheckPoint(Vector3 newPos)
    {
        if (PhotonNetwork.IsMasterClient || !isOnline)
        {
            inst.CheckPoint = newPos;
            if (isOnline)
            {
                var hash = PhotonNetwork.CurrentRoom.CustomProperties;
                if(hash.ContainsKey("CheckPoint"))
                {
                    hash["CheckPoint"] = JsonUtility.ToJson(newPos);
                }
                else if(!hash.ContainsKey("CheckPoint"))
                {
                    hash.Add("CheckPoint", JsonUtility.ToJson(newPos));
                }
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }
        }
    }

    public static GameObject GetLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
            return players[0];

        foreach (GameObject player in players)
        {
            // Debug.Log(player.name);
            if (!isOnline)
            {
                return player;
            }
            else if (player.GetComponent<Photon.Pun.PhotonView>().IsMine)
            {
                return player;
            }
        }

        return null;
    }

    public static GameObject GetClosestTarget(Transform pos)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject result = pos.gameObject;
        float distance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float newDistance = Vector3.Distance(pos.position, player.transform.position);
            if (newDistance <= distance)
            {
                distance = Mathf.Abs(newDistance);
                result = player;
            }
        }

        return result;
    }

    public static void SetEventReference(string val)
    {
        eventRef = val;
    }
    public static void SaveGame()
    {
        if (!GameManager.isOnline || PhotonNetwork.IsMasterClient)
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
            if (eventRef != "")
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
}
