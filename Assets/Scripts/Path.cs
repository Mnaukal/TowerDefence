using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] Points;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < Points.Length - 1; i++)
        {
            if(Points[i] != null && Points[i+1] != null)
                Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
        }
    }

}
