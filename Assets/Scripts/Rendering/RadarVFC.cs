using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Optimization_Module
{
    /// <summary>
    /// Class in charge of adding frustrum optimization technique by only rendering object nearby and on the players POV.
    /// </summary>
    public class RadarVFC : MonoBehaviour
    {
        public float minrenderDistance = 15, staticRender =20;
        Camera cam;
        public List<RenderObject> SceneObjs;

        bool Frustum(GameObject anyObject)
        {
            if(anyObject.isStatic && Vector3.Distance(anyObject.transform.position, this.transform.position) < staticRender)
            {
                return true;
            }

            if (Vector3.Distance(anyObject.transform.position, this.transform.position) < minrenderDistance)
            {
               return true;
            }
            Vector3 CAMERA = cam.transform.position;
            Vector3 Camx = cam.transform.right;
            Vector3 Camy = cam.transform.up;
            Vector3 Camz = cam.transform.forward;

            float near = cam.nearClipPlane;
            float far = cam.farClipPlane;
            float wd = cam.pixelWidth;
            float ht = cam.pixelHeight;


            Vector3 objPos = anyObject.transform.position;
            
            if (Vector3.Distance(objPos, this.transform.position) > (far/6)*5)
            {
                return false;
            }

            Vector3 w = new Vector3(objPos.x - CAMERA.x, objPos.y - CAMERA.y, objPos.z - CAMERA.z);

            if (Vector3.Dot(w, Camz) < near || Vector3.Dot(w, Camz) > far)
            {
                return false;
            }
            else if (Vector3.Dot(w, Camy) < -ht / 10 || Vector3.Dot(w, Camy) > ht / 10)
            {
                return false;
            }
            else if (Vector3.Dot(w, Camx) < -wd / 2 || Vector3.Dot(w, Camx) > wd / 2)
            {
                return false;
            }

            return true;
        }

        // Start is called before the first frame update
        void Start()
        {
            cam = this.GetComponent<Camera>(); //GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        // Update is called once per frame

        void Update()
        {
            foreach (RenderObject render in SceneObjs)
            {
                render.SetRender(Frustum(render.gameObject));
            }
        }

        /// <summary>
        /// Method that removes an RenderObject object to the SceneObjs rendering list.
        /// </summary>
        /// <param name="i">RenderObject object to be removed.</param>
        public void RemoveSceneObj(RenderObject i)
        {
            SceneObjs.Remove(i);
        }

        /// /// <summary>
        /// Method that adds a RenderObject object to the SceneObjs rendering list.
        /// </summary>
        /// <param name="i"> RenderObject object to be render. </param>
        public void AddSceneObj(RenderObject i)
        {
            SceneObjs.Add(i);
        }

    }
}
