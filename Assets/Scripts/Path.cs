using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    public Transform[] Points;
    private LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        Vector3[] positions = Points.Select(p => p.position).ToArray();
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        edgeCollider.points = Functions.Vector3ToVector2(positions);
    }

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
