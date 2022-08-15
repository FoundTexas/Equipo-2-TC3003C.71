using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] points;
    public int point;

    void Update()
    {
        transform.Translate(
            (points[point].position.normalized - transform.position.normalized)
            * speed * Time.deltaTime);

        if (Vector3.Distance(points[point].position, this.transform.position)<= 0.1f)
        {
            point++;
            
            if(point >= points.Length)
            {
                point = 0;
            }
        }
    }
}
