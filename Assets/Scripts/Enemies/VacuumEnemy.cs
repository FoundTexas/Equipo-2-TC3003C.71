using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    /// <summary>
    /// Class of the vacuum enemy that inherits Enemy behaviour.
    /// </summary>
    public class VacuumEnemy : Enemy
    {
        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Awake()
        {
            // Initialize private components
            GameObject _player = GameObject.FindWithTag("Player");
            if(_player)
                player = _player.transform;
            
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            GameObject manager = GameObject.FindWithTag("Manager");
            if(manager!=null)
                hitStop = manager.GetComponent<HitStop>();
        }

        void Update()
        {
            Patrolling();
        }
    }
}
