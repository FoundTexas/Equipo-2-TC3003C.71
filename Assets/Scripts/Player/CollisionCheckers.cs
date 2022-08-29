using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckers : MonoBehaviour
{
    private bool isCollide;
    public List<string> excTags;
    BoxCollider collid;

    private void Start()
    {
        collid = this.GetComponent<BoxCollider>();
    }
    private void Update()
    {
      
        isCollide = checkCol();

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!excTags.Exists(x => x.ToLower().Equals(other.tag.ToLower())))
    //    {
    //        isCollide = true && checkCol(); 
    //    }
        
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if(!excTags.Exists(x => x.ToLower().Equals(other.tag.ToLower())))
    //    {
    //        isCollide = true && checkCol();
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (!excTags.Exists(x => x.ToLower().Equals(other.tag.ToLower())))
    //    {
    //        isCollide = false;
    //    }
    //}

    public bool inCollide()
    {
        return isCollide;
    }

    private bool checkCol()
    {
        Collider[] colliders = Physics.OverlapBox(
                center: collid.transform.position + (collid.transform.rotation * collid.center),
                halfExtents: Vector3.Scale(collid.size * 0.5f, collid.transform.lossyScale),
                orientation: collid.transform.rotation);
        List<Collider> newColls = new List<Collider>();
        foreach (Collider col in colliders)
        {
            if (!excTags.Exists(x => x.ToLower().Equals(col.tag.ToLower())))
            { 
               newColls.Add(col);
            }
        }
        if (newColls.Count == 0)
        {
            return false;
        }
        return true;
    }

}
