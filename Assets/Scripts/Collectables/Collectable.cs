using GameManagement;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// Class assigned to all static Collectables that stores the collected value changing the in game appearance.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class Collectable : MonoBehaviour
    {
        [JsonIgnore] public Material mat, mat2;
        [Tooltip("Renderer object to vizualize mesh")]
        public MeshRenderer render;
        [Tooltip("Collected value of the Collectable")]
        [SerializeField] private bool isCollected;
        [Tooltip("Pick up effect")]
        [SerializeField] private GameObject effect;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            render = GetComponent<MeshRenderer>();
            // Debug.Log(JsonUtility.ToJson(this));
        }

        private void Update()
        {
            render.material = isCollected ? mat2 : mat;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isCollected = true;
                FindObjectOfType<SceneManagement>().SaveValue(this);
                gameObject.SetActive(false);
            }
        }

        // ----------------------------------------------------------------------------------------------- Public Methods

        /// <summary>
        /// Method that change the collected value.
        /// </summary>
        /// <param name="isCollected"> Bollean new value to isCollected. </param>
        public void SetCollected(bool isCollected)
        {
            this.isCollected = isCollected;
        }

        /// <summary>
        /// Method that gets the collected private value.
        /// </summary>
        /// <returns> isCollected boolean value. </returns>
        public bool GetCollected() 
        { 
            return isCollected; 
        }
    }
}
