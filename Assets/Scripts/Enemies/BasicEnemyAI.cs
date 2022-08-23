using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    //Variables needed for enemy calculations
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;


    //Patrolling variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking variables
    public float attackSpeed;
    bool hasAttacked;

    //State variables
    public float sightRange, attackRange;
    public bool playerInSights, playerInRange;

    Animator animator;

    void Awake()
    {
        player = GameObject.Find("Robot").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check sight and attack range
        playerInSights = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
    
        //Set appropriate state based on current position of player
        //in comparison to enemy
        if(!playerInSights && !playerInRange)
            Patrolling();
        if(playerInSights && !playerInRange)
            Chasing();
        if(playerInSights && playerInRange)
            Attacking();
    
    
    }

    void Patrolling()
    {
        if(!walkPointSet)
            CreateWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToGoal = transform.position - walkPoint;

        //Reached the destination, repeat to find a new one
        if(distanceToGoal.magnitude < 1f)
            walkPointSet = false;
    }

    void Chasing()
    {
        agent.SetDestination(player.position);
    }

    void Attacking()
    {
        //Stop enemy movement and look at player
        agent.SetDestination(transform.position);
        animator.SetTrigger("Attack");
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        transform.LookAt(targetPosition);


        if(!hasAttacked)
        {
            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackSpeed);
        }

    }

    void CreateWalkPoint()
    {
        //Generate a random point in enemy range
        float randX = Random.Range(-walkPointRange, walkPointRange);
        float randZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true;
    }

    void ResetAttack()
    {
        hasAttacked = false;
    }

}
