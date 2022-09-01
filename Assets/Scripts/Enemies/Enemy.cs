using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage
{
    //Variables needed for enemy calculations
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;


    //Patrolling variables
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking variables
    public float attackSpeed;
    public bool hasAttacked;

    //State variables
    public float sightRange, attackRange;
    public bool playerInSights, playerInRange;

    public Animator animator;

    [SerializeField] float hp = 20;
    float maxhp;
    [SerializeField] Renderer render;

    void Awake()
    {
        maxhp = hp;
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
        else if(playerInSights && !playerInRange)
            Chasing();
        else if(playerInSights && playerInRange)
            Attacking();
    
    
    }

    public virtual void Patrolling()
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

    public void Chasing()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            agent.SetDestination(player.position);    
    }

    public virtual void Attacking()
    {
        //Stop enemy movement and look at player
        agent.SetDestination(transform.position);

        //Find the player's current position
        //Keep enemy's current position in Y axis to prevent tilt
        //Rotate enemy to face player
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);

        if(!hasAttacked)
        {
            animator.SetTrigger("Attack");
            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackSpeed);
        }

    }

    public virtual void CreateWalkPoint()
    {
        //Generate a random point in enemy range
        float randX = Random.Range(-walkPointRange, walkPointRange);
        float randZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
            
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public virtual void TakeDamage(float dmg)
    {
        hp -= dmg;
        render.material.color = new Color(hp / maxhp,1, hp / maxhp);

        if (hp < 0)
        {
            Die();
        }
    }

    public void Freeze()
    {
        throw new System.NotImplementedException();
    }

    public void Burn()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Burnning()
    {
        throw new System.NotImplementedException();
    }
}
