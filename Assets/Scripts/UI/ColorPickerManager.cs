using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerManager : MonoBehaviour
{
    public GameObject[] elements;
    private int currentElement = 0;
    
    void Start()
    {
        SetElement(0);
    }

    public void Forward()
    {
        currentElement++;
        if(currentElement >= elements.Length){ currentElement = 0; }
        SetElement(currentElement);
    }

    public void Backwards()
    {
        currentElement--;
        if(currentElement < 0){ currentElement = elements.Length - 1; }
        SetElement(currentElement);
    }

    public void SetElement(int element)
    {
        elements[element].SetActive(true);
        for (int i = 0; i < elements.Length; i++)
            if(i != element)
            {
                elements[i].SetActive(false);
            }
    }
}
