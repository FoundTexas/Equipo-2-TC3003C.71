using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameManagement;
using WeaponSystem;

namespace Enemies
{
    /// <summary>
    /// Class of the vacuum enemy that inherits Enemy behaviour.
    /// </summary>
    public class SecurityEnemy : Enemy
    {
        RaycastHit rayHit;
        public GameObject pParent;
        public GameObject playerObj;
        public List<Transform> patrolPoints;
        public int currentPoint = 0;
        public bool capturing = false;
        public Move playerMove;
        public Animator playerAnimator;
        public bool frozenPlayer = false;
        public FadeBlack fade;
        private SceneLoader sceneLoader;
        public WeaponManager weapons;
        public GameObject conductor;
        public float viewRange = 30f;
        public float jumpDelay = 2f;
        public float jumpTimer = 0f;
        public bool willSleep = false;
        public bool asleep = false;
        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Awake()
        {
            // Initialize private components
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            playerObj = GameObject.FindWithTag("Player");
            player = playerObj.transform;
            weapons = playerObj.GetComponentInChildren<WeaponManager>();
            playerAnimator = player.GetComponentsInChildren<Animator>()[1];
            playerMove = player.GetComponent<Move>();
            fade = GameObject.FindWithTag("GameCanvas").GetComponentsInChildren<FadeBlack>()[0];
            GameObject manager = GameObject.FindWithTag("Manager");
            sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
            if(manager!=null)
                hitStop = manager.GetComponent<HitStop>();
            foreach(Transform child in pParent.transform)
            {
                patrolPoints.Add(child);
            }
        }

        void Update()
        {
            if(asleep && !capturing)
                return;
            jumpTimer += Time.deltaTime;
            if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Jump_CactusPal"))
                    agent.Resume();
            else
            {
                agent.Stop();
                agent.velocity = Vector3.zero;
            }

            conductor = GameObject.FindWithTag("Enemy");
            if(conductor != null)
            {
                Destroy(gameObject);
                return;
            } 
            if(weapons.CurrentSelect().curShootS >= weapons.CurrentSelect().shootSpeed)
                Capture();
                
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit, viewRange))
            {
                if(rayHit.collider.tag == "Prop"){}
                else if(rayHit.collider.tag == "Player")
                {
                    if(!frozenPlayer)
                    {
                        playerMove.canMove = false;
                        playerMove.StopMove();
                        playerAnimator.SetTrigger("ArmRaise");
                        capturing = true;
                        frozenPlayer = true;
                    }      
                }
            }
            if(capturing)
                Capture();
            else
                Patrolling();
            
        }

        public override void Patrolling()
        {
            if(!walkPointSet)
                CreateWalkPoint();

            if(walkPointSet)
            {
                agent.SetDestination(walkPoint);
                if(jumpTimer >= jumpDelay)
                {
                    animator.SetTrigger("Jump");
                    jumpTimer = 0f;
                }
                
            }
                

            Vector3 distanceToGoal = transform.position - walkPoint;

            //Reached the destination, repeat to find a new one
            if(distanceToGoal.magnitude < 1f)
            {
                walkPointSet = false;
                if(willSleep)
                    asleep = true;
            }
        }

        public override void CreateWalkPoint()
        {
            if(currentPoint == patrolPoints.Count-1)
                currentPoint = -1;
            currentPoint += 1;
            walkPoint = patrolPoints[currentPoint].position;

            walkPointSet = true;
        }

        public void Capture()
        {
            if(!frozenPlayer)
            {
                playerMove.canMove = false;
                playerMove.StopMove();
                playerAnimator.SetTrigger("ArmRaise");
                capturing = true;
                frozenPlayer = true;
            }
            agent.SetDestination(transform.position);
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);
            fade.DoFade(true, 0.5f);
            if(fade.IsFaded())
                sceneLoader.LoadByIndex(GameManager.getSceneIndex(), GameManager.getCheckpoint());
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.tag == "Player")
            {
                Capture();
            }
        }
    }

    
}
