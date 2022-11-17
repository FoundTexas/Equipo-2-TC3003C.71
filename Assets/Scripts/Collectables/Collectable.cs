using GameManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectables
{
    [Serializable]
    public class CollectableData
    {
        [Tooltip("Collected value of the Collectable")]
        [SerializeField] public bool isCollected;
    }
    /// <summary>
    /// Class assigned to all static Collectables that stores the collected value changing the in game appearance.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class Collectable : MonoBehaviour
    {
        public int index;
        public Material mat, mat2;
        [Tooltip("Renderer object to vizualize mesh")]
        [NonSerialized] public MeshRenderer render;

        public CollectableData data;

        [Tooltip("Pick up effect")]
        [NonSerialized] private GameObject effect;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        private void Start()
        {
            render = GetComponent<MeshRenderer>();
            // Debug.Log(JsonUtility.ToJson(this));
        }

        private void Update()
        {
            render.material = data.isCollected ? mat2 : mat;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                data.isCollected = true;
                GameManager.SaveGame();
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
            data.isCollected = isCollected;
        }

        /// <summary>
        /// Method that gets the collected private value.
        /// </summary>
        /// <returns> isCollected boolean value. </returns>
        public bool GetCollected()
        {
            return data.isCollected;
        }
    }
}
