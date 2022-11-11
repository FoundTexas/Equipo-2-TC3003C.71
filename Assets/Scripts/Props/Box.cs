using Collectables;
using Interfaces;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Props
{
    public class Box : MonoBehaviour, IDamage
    {
        PhotonView pv;
        [Header("Box Stats")]

        [Tooltip("The max amount of hit points of the box")]
        public float maxhp = 20;
        [Tooltip("Prefab of the destroyed Box")]
        public GameObject ExplosiveCrate;
        float hp = 20;
        Renderer render;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            hp = maxhp;
            render = GetComponent<Renderer>();
        }
        [PunRPC]
        void PunRPCupdateVisuals()
        {
            render.material.color = new Color(1, hp / maxhp, hp / maxhp);
        }

        // ----------------------------------------------------------------------------------------------- Interface IDamage

        /// <summary>
        /// Interface Abstract method in charge of the death routine of the assigned Object.
        /// </summary>
        public void Die()
        {
            GetComponent<Dropper>().Spawn();
            Instantiate(ExplosiveCrate, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        /// <summary>
        /// Interface Abstract method that handels when an object takes damage.
        /// </summary>
        /// <param name="dmg"> Amount of damage taken. </param>
        public void TakeDamage(float dmg)
        {
            if (GameManager.isOnline)
            {
                pv.RPC("PunRPCupdateVisuals", RpcTarget.All);
                pv.RPC("TakeDamageRPC", RpcTarget.All, dmg);
            }
            else if (!GameManager.isOnline)
            {
                PunRPCupdateVisuals();
                TakeDamageRPC(dmg);
            }
        }
        [PunRPC]
        void TakeDamageRPC(float dmg)
        {
            hp -= dmg;

            if (hp <= 0)
            {
                Die();
            }
        }
        
        /// <summary>
        /// Interface Abstract method that starts the freezing of an object.
        /// </summary>
        public void Freeze()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Interface Abstract method that starts the burnning of an object.
        /// </summary>
        public void Burn()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Interface Abstract method that handels the burnning of an object.
        /// </summary>
        public IEnumerator Burnning()
        {
            throw new System.NotImplementedException();
        }
    }
}
