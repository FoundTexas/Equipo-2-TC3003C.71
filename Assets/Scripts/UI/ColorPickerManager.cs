using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetCrashUI
{
    public class ColorPickerManager : MonoBehaviour
    {
        [Tooltip("References to the Color Picker elements on main menu")]
        public GameObject[] elements;
        private int currentElement = 0;

        // ----------------------------------------------------------------------------------------------- Unity Methods

        void Start()
        {
            SetElement(0);
        }

        // ----------------------------------------------------------------------------------------------- Public Methods
        /// <summary>
        /// Goes forward on the index.
        /// </summary>
        public void Forward()
        {
            currentElement++;
            if (currentElement >= elements.Length) { currentElement = 0; }
            SetElement(currentElement);
        }
        /// <summary>
        /// Goes backs on the index.
        /// </summary>
        public void Backwards()
        {
            currentElement--;
            if (currentElement < 0) { currentElement = elements.Length - 1; }
            SetElement(currentElement);
        }
        /// <summary>
        /// Method that sets the Color picker elements active or inactive.
        /// </summary>
        /// <param name="element"> the active element index. </param>
        public void SetElement(int element)
        {
            elements[element].SetActive(true);
            for (int i = 0; i < elements.Length; i++)
                if (i != element)
                {
                    elements[i].SetActive(false);
                }
        }
    }
}
