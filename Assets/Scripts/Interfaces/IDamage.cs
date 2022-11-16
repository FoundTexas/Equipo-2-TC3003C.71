using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Interfaces
{
    /// <summary>
    /// Interface that handels Damage between Objects.
    /// </summary>
    public interface IDamage
    {
        /// <summary>
        /// Interface Abstract method used to give and object a dead routine.
        /// </summary>
        [PunRPC]
        public void PunRPCDie();
        /// <summary>
        /// Interface Abstract method that handels when an object takes damage.
        /// </summary>
        /// <param name="dmg"> Amount of damage taken. </param>
        public void TakeDamage(float dmg);
        /// <summary>
        /// Interface Abstract method that starts the freezing of an object.
        /// </summary>
        public void Freeze();
        /// <summary>
        /// Interface Abstract method that starts the burnning of an object.
        /// </summary>
        public void Burn();
        /// <summary>
        /// Interface Abstract method that handels the burnning of an object.
        /// </summary>
        public IEnumerator Burnning();
    }
}
