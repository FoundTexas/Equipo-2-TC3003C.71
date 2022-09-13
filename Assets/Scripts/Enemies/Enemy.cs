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

    public HitStop hitStop;
    public GameObject explosionfx;
    public float timePatrolling = 0f;

    void Awake()
    {
        maxhp = hp;
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GameObject manager = GameObject.FindWithTag("Manager");
        if(manager!=null)
            hitStop = manager.GetComponent<HitStop>();
            
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

        timePatrolling += Time.deltaTime;

        //Reached the destination, repeat to find a new one
        if(distanceToGoal.magnitude < 1f || timePatrolling >= 3f)
        {
            walkPointSet = false;
            timePatrolling = 0f;
        }
        
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
        GetComponent<Dropper>().Spawn();
        GameObject deathvfx;
        Vector3 vfxpos = this.transform.position;
        vfxpos.y = this.transform.position.y + 1;
        deathvfx = Instantiate(explosionfx, vfxpos, Quaternion.identity);

        Destroy(this.gameObject);
        if(hitStop != null)
            hitStop.HitStopFreeze(0.02f, 0.2f);

        var vfxDuration = 1f;
        Destroy(deathvfx, vfxDuration);
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
