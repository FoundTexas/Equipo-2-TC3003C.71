using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    /// <summary>
    /// Class of the vacuum enemy that inherits Enemy behaviour.
    /// </summary>
    public class SecurityEnemy : Enemy
    {
        RaycastHit rayHit;
        public GameObject pParent;
        public List<Transform> patrolPoints;
        public int currentPoint = 0;
        public bool capturing = false;
        public Move playerMove;
        public Animator playerAnimator;
        public bool frozenPlayer = false;
        public FadeBlack fade;
        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Awake()
        {
            // Initialize private components
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            playerAnimator = player.GetComponentsInChildren<Animator>()[1];
            playerMove = player.GetComponent<Move>();
            fade = GameObject.FindWithTag("GameCanvas").GetComponentsInChildren<FadeBlack>()[0];
            GameObject manager = GameObject.FindWithTag("Manager");
            if(manager!=null)
                hitStop = manager.GetComponent<HitStop>();
            foreach(Transform child in pParent.transform)
            {
                patrolPoints.Add(child);
            }
        }

        void Update()
        {
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit))
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
                agent.SetDestination(walkPoint);

            Vector3 distanceToGoal = transform.position - walkPoint;

            //Reached the destination, repeat to find a new one
            if(distanceToGoal.magnitude < 1f)
            {
                walkPointSet = false;
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
            agent.SetDestination(transform.position);
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);
            fade.DoFade(true, 0.5f);
            if(fade.IsFaded())
                Debug.Log("Die.");
        }
    }

    
}
