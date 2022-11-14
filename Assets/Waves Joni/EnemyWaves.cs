using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyWaves : MonoBehaviour
{
    private Waves waves;

    public NavMeshAgent enemyAgent;
    public float chaseRadius = 25f;
    public Transform mapCenter;
    private Vector3 destPos;
    public Transform playerTransform;
    public Transform playerTransform2;
   
    private void Start()
    {
        enemyAgent = transform.GetComponent<NavMeshAgent>();
        enemyAgent.speed = 5f;
        waves = GameObject.Find("EnemyWave").GetComponent<Waves>();
        playerTransform = waves.GetPlayerPos();
        mapCenter = waves.GetCenterPos();
        destPos = mapCenter.position;
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= chaseRadius)
        {
            destPos = playerTransform.position;
            //print("player in sight");
        }else if(Vector3.Distance(transform.position, playerTransform.position) >= chaseRadius)
        {
            //print("player out of sight");
            destPos = mapCenter.position;
        }
        enemyAgent.SetDestination(destPos);
    }

}
