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
        bool isDestroyable;
        bool isApplicationQuitting = false;
        Renderer render;

        // Start is called before the first frame update
        void Start()
        {
            render = GetComponent<Renderer>();
            FindObjectOfType<RadarVFC>().AddSceneObj(this);
        }

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

        private void OnDestroy()
        {
            if (isApplicationQuitting)
            {
                return;
            }
            else  if (!isApplicationQuitting)
            {
                FindObjectOfType<RadarVFC>().RemoveSceneObj(this);
            }
        }

        void OnApplicationQuit()
        {
            isApplicationQuitting = true;
        }
    }
}
