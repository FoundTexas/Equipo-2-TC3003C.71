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
        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Awake()
        {
            // Initialize private components
            player = GameObject.FindWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            GameObject manager = GameObject.FindWithTag("Manager");
            if(manager!=null)
                hitStop = manager.GetComponent<HitStop>();
            foreach(Transform child in pParent.transform)
            {
                patrolPoints.Add(child);
                Debug.Log("Added child.");
            }
        }

        void Update()
        {
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit))
            {
                if(rayHit.collider.tag == "Prop"){}
                else if(rayHit.collider.tag == "Player")
                {
                    Debug.Log("Target Spotted");
                }
            }
            Patrolling();
            
        }

        public override void Patrolling()
        {
            if(!walkPointSet)
                CreateWalkPoint();

            if(walkPointSet)
                agent.SetDestination(walkPoint);

            Vector3 distanceToGoal = transform.position - walkPoint;

            timePatrolling += Time.deltaTime;

            //Reached the destination, repeat to find a new one
            if(distanceToGoal.magnitude < 1f)
            {
                walkPointSet = false;
                timePatrolling = 0f;
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
    }
}
