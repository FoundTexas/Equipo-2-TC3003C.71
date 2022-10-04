using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagement
{
    /// <summary>
    /// Class that handels the enabling of a UnityEvent when destroying this script in order affect the game in some sort.
    /// </summary>
    public class TriggerEventOnDead : InGameEvent
    {
        bool isApplicationQuitting = false;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void OnDestroy()
        {
            if (isApplicationQuitting)
            {
                return;
            }
            else if (!isApplicationQuitting)
            {
                SetTrigger();
            }
        }

        void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }
    }
}
