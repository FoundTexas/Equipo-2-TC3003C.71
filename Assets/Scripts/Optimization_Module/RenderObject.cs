using Optimization_Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Optimization_Module
{
    /// <summary>
    /// This class handels if an object enters the RadarVFC render queue.
    /// </summary>
    public class RenderObject : MonoBehaviour
    {
        public bool canDestroy = false;
        bool isApplicationQuitting = false;
        Renderer render;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void Start()
        {
            render = GetComponent<Renderer>();
            FindObjectOfType<RadarVFC>().AddSceneObj(this);
        }
        private void OnDestroy()
        {
            // if (canDestroy)
            // {
            //     if (isApplicationQuitting)
            //     {
            //         return;
            //     }
            //     else if (!isApplicationQuitting)
            //     {
            //         FindObjectOfType<RadarVFC>().RemoveSceneObj(this);
            //     }
            // }
        }
        void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// This method change the object active state based on the frustrum result from RadarVFC.
        /// </summary>
        /// <param name="isActive"> Boolean that sets active state. </param>
        public void SetRender(bool isActive)
        {
            if (render)
            {
                render.enabled = isActive;
            }
            else
            {
                this.gameObject.SetActive(isActive);
            }
        }
    }
}
