using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemUpdater : MonoBehaviour
{
    public EventSystem updater;
    GameObject referance;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        referance = updater.firstSelectedGameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            if (!updater.currentSelectedGameObject)
            {
                updater.SetSelectedGameObject(referance.gameObject);
            }
            else if (updater.currentSelectedGameObject)
            {
                if(referance){
                referance = updater.currentSelectedGameObject;
                }
                else{
                    referance = updater.firstSelectedGameObject;
                }
            }
        }
        else if (!paused)
        {
            updater.SetSelectedGameObject(null);
        }
    }

    public void OnPause(bool pause)
    {
        paused = pause;
    }

    public void UpdateSelected(GameObject select){
        referance = select;
        updater.SetSelectedGameObject(referance.gameObject);
    }
}
