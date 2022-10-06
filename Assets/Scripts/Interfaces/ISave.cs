using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// Interface that handels Saving the objects via Json
    /// </summary>
    public interface ISave
    {
        /// <summary>
        /// Interface Abstract method that gets an object from a Json.
        /// </summary>
        public void FromJson();

        /// <summary>
        /// Interface Abstract method that makes sure the object was saved.
        /// </summary>
        /// <returns> If saved returns true. </returns>
        public bool Save();
    }
}
