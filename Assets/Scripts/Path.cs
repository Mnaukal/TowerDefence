using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents the path along which enemies walk
/// </summary>
public class Path : MonoBehaviour
{
    //Public
    /// <summary>
    /// Points of the path; path is composed of line segments between these points
    /// </summary>
    public Transform[] Points;
    //Private
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    # region Events
    /// <summary>
    /// Called when a collider enters the path
    /// </summary>
    public event TriggerEventHandler TriggerEnter;
    /// <summary>
    /// Called when a collider exits the path
    /// </summary>
    public event TriggerEventHandler TriggerExit;

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
        // setup rendering and collider of the path
        Vector3[] positions = Points.Select(p => p.position).ToArray();
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        edgeCollider.points = Functions.Vector3ToVector2(positions);
    }

    private void OnDrawGizmos()
    {
        // Draw path in the Editor
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
