using Collectables;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace Enemies
{
    /// <summary>
    /// Main class of the enemy that manages its behaviour.
    /// </summary>
    public class Enemy : MonoBehaviour, IDamage
    {
        [Header("Enemy calculations")]
        public NavMeshAgent agent;
        public Transform player;
        public LayerMask isGround, isPlayer;

        [Header("Patrolling variables")]
        public Vector3 walkPoint;
        public bool walkPointSet;
        public float walkPointRange;

        [Header("Attacking variables")]
        public float attackSpeed;
        public bool hasAttacked;

        [Header("State variables")]
        public float sightRange, attackRange;
        public bool playerInSights, playerInRange;
        public Animator animator;
        public HitStop hitStop;
        public GameObject explosionfx;
        public float timePatrolling = 0f;
        [SerializeField] private float hp = 20;
        [SerializeField] private Renderer render;
        private float maxHp;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Awake()
        {
            
            // Initialize private components
            //player = GameObject.FindWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            GameObject manager = GameObject.FindWithTag("Manager");
            if (manager != null)
                hitStop = manager.GetComponent<HitStop>();

            // Establish original values
            maxHp = hp;
        }

        void Update()
        {
            player = GameManager.GetClosestTarget(transform.position).transform;
            Debug.Log(GameManager.isOnline);
            Debug.Log(TimelineManager.enemiesCanMove);

            if (!GameManager.isOnline || PhotonNetwork.IsMasterClient)
            {
                if (TimelineManager.enemiesCanMove)
                {
                    //Check sight and attack range
                    playerInSights = Physics.CheckSphere(transform.position, sightRange, isPlayer);
                    playerInRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

                    //Set appropriate state based on current position of player in comparison to enemy
                    if (!playerInSights && !playerInRange)
                    {
                        Patrolling();
                    }
                    else if (playerInSights && !playerInRange)
                        Chasing();
                    else if (playerInSights && playerInRange)
                        Attacking();
                }
            }
        }
        public virtual void Patrolling()
        {
            if (!walkPointSet)
                CreateWalkPoint();

            if (walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToGoal = transform.position - walkPoint;

            timePatrolling += Time.deltaTime;

            //Reached the destination, repeat to find a new one
            if (distanceToGoal.magnitude < 1f || timePatrolling >= 3f)
            {
                walkPointSet = false;
                timePatrolling = 0f;
            }

        }

        public void Chasing()
        {
            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                agent.SetDestination(player.position);
        }

        public virtual void Attacking()
        {
            //Stop enemy movement and look at player
            agent.SetDestination(transform.position);

            //Find the player's current position
            //Keep enemy's current position in Y axis to prevent tilt
            //Rotate enemy to face player
            player = GameManager.GetClosestTarget(transform.position).transform;
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);

            if (!hasAttacked)
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

            if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            {
                walkPointSet = true;
            }

        }

        public void ResetAttack()
        {
            hasAttacked = false;
        }

        // ----------------------------------------------------------------------------------------------- Interface IDamage
        /// <summary>
        /// Interface Abstract method in charge of the death routine of the assigned Object.
        /// </summary>
        [PunRPC]
        public void PunRPCDie()
        {
            GameObject deathvfx;
            Vector3 vfxpos = this.transform.position;
            vfxpos.y = this.transform.position.y + 1;
            deathvfx = Instantiate(explosionfx, vfxpos, Quaternion.identity);

            Destroy(this.gameObject);
            if (hitStop != null)
                hitStop.HitStopFreeze(3f, 0.2f);

            var vfxDuration = 1f;
            GetComponent<Dropper>().Spawn();
            Destroy(deathvfx, vfxDuration);
        }

        /// <summary>
        /// Interface Abstract method that handels when an object takes damage.
        /// </summary>
        /// <param name="dmg"> Amount of damage taken. </param>
        public virtual void TakeDamage(float dmg)
        {
            hp -= dmg;
            //render.material.color = new Color(hp / maxHp, 1, hp / maxHp);
            CameraShake.Instance.DoShake(0.5f, 1f, 0.1f);
            if (hp < 0)
                PunRPCDie();
        }

        /// <summary>
        /// Interface Abstract method that starts the freezing of an object.
        /// </summary>
        public void Freeze()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Interface Abstract method that starts the burnning of an object.
        /// </summary>
        public void Burn()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Interface Abstract method that handels the burnning of an object.
        /// </summary>
        public IEnumerator Burnning()
        {
            throw new System.NotImplementedException();
        }
    }
}
