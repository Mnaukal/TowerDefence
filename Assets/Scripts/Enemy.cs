using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    // Public
    public float speed = 1f;
    // Private
    private Path path;
    private int pathNodeProgress = 0;
    private float pathProgress = 0;
    private float segmentLength;
    private bool waiting = false; // Wait after slower enemy before me
    private List<Enemy> waitingFor = new List<Enemy>();

    #region Events
    public event EnemyFinishedEventHandler EnemyFinished;

    private void RaiseEnemyFinished()
    {
        if (EnemyFinished != null)
            EnemyFinished(this, new EnemyFinishedEventArgs(this));
    }

    #endregion

    private void Awake()
    {
        path = GameControllerS.I.Path;
    }

    private void Start()
    {
        SetupSegment(0);
    }

    private void Update()
    {
        if(!waiting)
            MoveEnemy();
    }

    void MoveEnemy()
    {
        float distDelta = Time.deltaTime * speed;
        pathProgress += distDelta / segmentLength;
        if (pathProgress >= 1)
            SetupSegment(pathNodeProgress + 1); // TODO event?
        transform.position = Vector3.Lerp(path.Points[pathNodeProgress].position, path.Points[pathNodeProgress + 1].position, pathProgress);
    }

    private void SetupSegment(int segmentIndex)
    {
        if (segmentIndex >= path.Points.Length - 1)
        {
            RaiseEnemyFinished();
            Destroy(this.gameObject);
            return;
        }

        pathNodeProgress = segmentIndex;
        pathProgress = 0f;
        segmentLength = Vector2.Distance(path.Points[pathNodeProgress].position, path.Points[pathNodeProgress + 1].position);
        transform.position = path.Points[pathNodeProgress].position;
        StartCoroutine(LookAtNextNode());
    }

    private IEnumerator LookAtNextNode()
    {
        Vector2 newRight = path.Points[pathNodeProgress + 1].position - transform.position;
        Vector2 currentRight = transform.right;

        for (float i = 0; i <= 1; i += Time.deltaTime / 0.3f)
        {
            transform.right = Vector2.Lerp(currentRight, newRight, i);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e == null)
            return;
        if (e.GetTotalProgress() > this.GetTotalProgress())
        {
            waiting = true;
            waitingFor.Add(e);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e == null)
            return;
        waitingFor.Remove(e);
        if (waitingFor.Count == 0)
            waiting = false;
    }

    /// <summary>
    /// Gets progress along the path.
    /// Value is 1 for every completed segment + progress on current segment (between 0 and 1). So the total range of this number is number of segments on the path.
    /// </summary>
    /// <returns></returns>
    public float GetTotalProgress()
    {
        return pathNodeProgress + pathProgress;
    }
}
