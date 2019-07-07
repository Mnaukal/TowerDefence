using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    //Public
    public Transform[] Points;
    //Private
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    # region Events
    public event TriggerEnterEventHandler TriggerEnter;
    public event TriggerExitEventHandler TriggerExit;

    private void RaiseTriggerEnter(Collider2D collision)
    {
        if (TriggerEnter != null)
            TriggerEnter(this, new TriggerEventArgs(collision));
    }
    private void RaiseTriggerExit(Collider2D collision)
    {
        if (TriggerExit != null)
            TriggerExit(this, new TriggerEventArgs(collision));
    }

    #endregion

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RaiseTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RaiseTriggerExit(collision);
    }

}
