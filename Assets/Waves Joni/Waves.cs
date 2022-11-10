using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameManagement;

public class Waves : MonoBehaviour
{
    public GameObject EnemyToSpawn;
    public Transform player;
    public Transform centerPos;
    public GameObject spawner;
    public int rounds;
    public int currentEnemies;
    public InGameEvent endEvent;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<int> numEnemy = new List<int>();
    bool isStarted = true;
    bool isEnded = false;

    void StartPoints()
    {
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j <= 360; j+=45/2)
            {
                GameObject spawn = Instantiate(spawner, transform.position, Quaternion.identity, transform);
                spawn.transform.Rotate(0,j,0);
                spawn.transform.localPosition += spawn.transform.forward * i * 5;
            }
        }
    }

    private void OnEnable()
    {
        isEnded = false;
        isStarted = true;
        StartPoints();
        rounds = 0;
    }

    private void FixedUpdate()
    {
        if (gameObject.activeInHierarchy && !isEnded)
        {
            StartRound();
        }
    }
    private void StartRound()
    {
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
            else if(currentEnemies <= 0 && isStarted)
            {
                isStarted = false;
                currentEnemies = 0;
                SpawnEnemies();
            }
        }
        else if (!isEnded)
        {
            isEnded = true;
            endEvent.SetEnded(true);
            GameManager.SaveGame();

        }
    }
    private void SpawnEnemies()
    {
        while (currentEnemies < numEnemy[rounds]) { 
           
            foreach (Transform points in transform)
            {
                SpawnerW spawnerW = points.GetComponent<SpawnerW>();
                if (spawnerW.GetSpawnBool())
                {
                    if (currentEnemies < numEnemy[rounds])
                    {
                        spawnedEnemies.Add(Instantiate(EnemyToSpawn, points.position, Quaternion.identity));
                        currentEnemies++;
                    }
                }
            }
        }
        rounds++;
        isStarted = true;
    }
    private IEnumerator SpawnEnemies_(Transform points)
    {
        float timeRandom = Random.Range(1f, 10f);
        yield return new WaitForSeconds(timeRandom);
        spawnedEnemies.Add(Instantiate(EnemyToSpawn, points.position, Quaternion.identity));
    }
    public Transform GetPlayerPos()
    {
        return player;
    }
    public Transform GetCenterPos()
    {
        return centerPos;
    }
}
