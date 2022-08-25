using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VacuumEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        player = GameObject.Find("Robot").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrolling();
    }


}
