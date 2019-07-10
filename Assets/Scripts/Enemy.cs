using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Base class for enemies
/// </summary>
public class Enemy : MonoBehaviour
{
    // Public parameters
    public float Speed = 1f;
    public int Health = 3;
    /// <summary>
    /// Money player gets when towers kill this enemy
    /// </summary>
    public int Reward = 1;
    // Private links to other objects
    [SerializeField]
    private Healthbar Healthbar;
    [SerializeField]
    private Transform Sprite;
    private Path path;
    // Private variables
    /// <summary>
    /// At which segment of path the enemy currently is
    /// </summary>
    private int pathNodeIndex = 0;
    /// <summary>
    /// Progress along current segment (0 to 1)
    /// </summary>
    private float pathSegmentProgress = 0;
    private float segmentLength;
    /// <summary>
    /// Don't move because there is a slower enemy in front
    /// </summary>
    private bool waiting = false;
    private List<Enemy> waitingFor = new List<Enemy>();

    #region Events
    /// <summary>
    /// Called when enemy finishes the path
    /// Player's HP should be decreased
    /// </summary>
    public event EnemyFinishedEventHandler EnemyFinished;
    /// <summary>
    /// Called when enemy gets hit by a projectile from a tower
    /// </summary>
    public event EnemyHitEventHandler EnemyHit;
    public event EnemyKilledEventHandler EnemyKilled;

    private void RaiseEnemyFinished()
    {
        if (EnemyFinished != null)
            EnemyFinished(this, new EnemyEventArgs(this));
    }

    private void RaiseEnemyHit(int damage, Vector2 hitPosition)
    {
        if (EnemyHit != null)
            EnemyHit(this, new EnemyHitEventArgs(this, damage, Health, hitPosition));
    }

    private void RaiseEnemyKilled()
    {
        if (EnemyKilled != null)
            EnemyKilled(this, new EnemyEventArgs(this));
    }
    #endregion

    /// <summary>
    /// Sets up enemy and starts it on first segment of path
    /// </summary>
    public void SetupEnemy(float speed, int health, int reward)
    {
        Speed = speed;
        Health = health;
        Reward = reward;

        SetupSegment(0);
        Healthbar.SetMax(Health);
        EnemyHit += Healthbar.EnemyHit;
    }

    private void Awake()
    {
        path = GameControllerS.I.Path;
    }

    private void Update()
    {
        if(!waiting)
            MoveEnemy();
    }

    /// <summary>
    /// Update enemy position along the path
    /// </summary>
    private void MoveEnemy()
    {
        float distDelta = Time.deltaTime * Speed;
        pathSegmentProgress += distDelta / segmentLength;
        if (pathSegmentProgress >= 1)
            SetupSegment(pathNodeIndex + 1); // TODO event?
        transform.position = Vector3.Lerp(path.Points[pathNodeIndex].position, path.Points[pathNodeIndex + 1].position, pathSegmentProgress);
    }

    /// <summary>
    /// Moves enemy to a new segment of path
    /// </summary>
    /// <param name="segmentIndex">index of path segment (from 0)</param>
    private void SetupSegment(int segmentIndex)
    {
        if (segmentIndex >= path.Points.Length - 1)
        {
            RaiseEnemyFinished();
            Destroy(this.gameObject);
            return;
        }

        pathNodeIndex = segmentIndex;
        pathSegmentProgress = 0f;
        segmentLength = Vector2.Distance(path.Points[pathNodeIndex].position, path.Points[pathNodeIndex + 1].position);
        transform.position = path.Points[pathNodeIndex].position;
        StartCoroutine(LookAtNextNode());
    }

    /// <summary>
    /// Over time rotates the enemy so it looks at next node in path
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator LookAtNextNode()
    {
        Vector2 newRight = path.Points[pathNodeIndex + 1].position - transform.position;
        Vector2 currentRight = Sprite.right;

        for (float i = 0; i <= 1; i += Time.deltaTime / 0.3f)
        {
            Sprite.right = Vector2.Lerp(currentRight, newRight, i);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy collision
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e != null) 
            CollisionEnterEnemy(e);

        // Projectile collision
        Projectile p = collision.gameObject.GetComponent<Projectile>();
        if (p != null)
            CollisionEnterProjectile(p);
    }

    /// <summary>
    /// Got hit by projectile
    /// </summary>
    private void CollisionEnterProjectile(Projectile p)
    {
        Hit(p.DamageEnemy(this), p.transform.position);
        p.ProjectileHit(this);
    }

    private void Hit(int damage, Vector2 hitPosition)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
        RaiseEnemyHit(damage, hitPosition);
    }

    private void Die()
    {
        RaiseEnemyKilled();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Bumped into enemy in front 
    /// </summary>
    private void CollisionEnterEnemy(Enemy e)
    {
        if (e.GetTotalProgress() > this.GetTotalProgress())
        {
            waiting = true;
            waitingFor.Add(e);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e != null)
            CollisionExitEnemy(e);
    }

    private void CollisionExitEnemy(Enemy e)
    {
        waitingFor.Remove(e);
        if (waitingFor.Count == 0)
            waiting = false;
    }

    /// <summary>
    /// Gets progress along the path.
    /// Value is 1 for every completed segment + progress on current segment (between 0 and 1). So the total range of this number is number of segments on the path.
    /// </summary>
    public float GetTotalProgress()
    {
        return pathNodeIndex + pathSegmentProgress;
    }
}

/// <summary>
/// Compare enemy progress along path to know which one is in front
/// </summary>
class EnemyTotalProgressComparer : IComparer<Enemy>
{
    Comparer<float> comparer;

    public EnemyTotalProgressComparer(Comparer<float> comparer)
    {
        this.comparer = comparer;
    }

    public EnemyTotalProgressComparer()
    {
        comparer = Comparer<float>.Default;
    }

    public int Compare(Enemy x, Enemy y)
    {
        return comparer.Compare(x.GetTotalProgress(), y.GetTotalProgress());
    }
}
