using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using Photon.Pun;

public class Waves : MonoBehaviour
{
    public GameObject EnemyToSpawn, forecfield;
    public Transform centerPos;
    public GameObject spawner;
    public int rounds;
    public int currentEnemies;
    public InGameEvent endEvent;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<Transform> spawnPoints = new List<Transform>();
    public List<int> numEnemy = new List<int>();
    bool isStarted = true;
    public static bool isEnded = true;
    bool allSpawned = false;
    private void Start()
    {
        isEnded = true;
    }
    void StartPoints()
    {
        Debug.Log("points 1");
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j <= 360; j += 45 / 4)
            {
                GameObject spawn = Instantiate(spawner, transform.position, Quaternion.identity, transform);
                spawn.transform.Rotate(0, j, 0);
                spawn.transform.localPosition += spawn.transform.forward * i * 2;
            }
        }
        foreach (Transform spawnPoint in transform)
        {
            spawnPoints.Add(spawnPoint);
        }
        spawnPoints.Reverse();
        Debug.Log("points 2");
    }

    private void OnEnable()
    {
        Debug.Log("Start spawn");
        allSpawned = false;
        isEnded = false;
        Debug.Log(isEnded);
        isStarted = true;
        StartPoints();
        rounds = 0;
    }

    private void FixedUpdate()
    {

        if (PhotonNetwork.IsMasterClient || !GameManager.isOnline)
        {
            Debug.Log(!isEnded);
            if (isStarted)
            {
                StartRound();
            }
        }
    }
    private void StartRound()
    {
        Debug.Log("Spawn");
        if (rounds < numEnemy.Count)
        {
            if (currentEnemies > 0)
            {
                currentEnemies = spawnedEnemies.Count;
                for (int i = 0; i < spawnedEnemies.Count; i++)
                {
                    if (!spawnedEnemies[i])
                    {
                        spawnedEnemies.RemoveAt(i);
                        return;
                    }
                }
            }
            else if (currentEnemies <= 0 && isStarted)
            {
                isStarted = false;
                currentEnemies = 0;
                StartCoroutine(SpawnEnemies());
            }
        }
        else if (!isEnded)
        {
            isEnded = true;
            endEvent.SetEnded(true);
            this.gameObject.SetActive(false);
            GameManager.SaveGame();
        }
    }
    private IEnumerator SpawnEnemies()
    {
        int currentEnem = 0;
        while (currentEnemies < numEnemy[rounds])
        {
            foreach (Transform points in spawnPoints)
            {
                float timeRandom = Random.Range(1f, 5f);
                int skipInt = ((int)Random.Range(1f, 10f));
                if (skipInt > 8)
                {
                    SpawnerW spawnerW = points.GetComponent<SpawnerW>();
                    if (spawnerW.GetSpawnBool())
                    {
                        if (currentEnemies < numEnemy[rounds])
                        {
                            StartCoroutine(SpawnEnemies_(points, timeRandom));
                            currentEnemies++;
                            currentEnem++;
                            if (currentEnem == numEnemy[rounds]) { allSpawned = true; }
                            yield return new WaitForSeconds(timeRandom + .5f);
                        }
                        else if (allSpawned)
                        {
                            Debug.Log("all");
                            if (currentEnemies == 0)
                            {
                                allSpawned = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        rounds++;
        isStarted = true;
    }
    private IEnumerator SpawnEnemies_(Transform points, float timeRandom)
    {
        yield return new WaitForSeconds(timeRandom);
        spawnedEnemies.Add(Instantiate(EnemyToSpawn, points.position, Quaternion.identity));
    }
    public Transform GetCenterPos()
    {
        return centerPos;
    }
}
