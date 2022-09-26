using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagement
{
    /// <summary>
    /// Class that handels the enabling of a UnityEvent on Trigger Collision in order affect the game in some sort.
    /// </summary>
    public class TriggerEventOnCollision : MonoBehaviour
    {
        [Tooltip("Set of instructions that affect other game objects")]
        public UnityEvent TriggerEvent;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SetTrigger();
            }
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Void in charge of instantiate the UnityEvent.
        /// </summary>
        public void SetTrigger()
        {
            TriggerEvent.Invoke();
        }
    }
}
