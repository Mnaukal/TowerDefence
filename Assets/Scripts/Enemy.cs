using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    // Public
    public float Speed = 1f;
    public int Health = 3;
    public int Reward = 1;
    public Healthbar Healthbar;
    public Transform Sprite;
    // Private
    private Path path;
    private int pathNodeProgress = 0;
    private float pathProgress = 0;
    private float segmentLength;
    private bool waiting = false; // Wait after slower enemy before me
    private List<Enemy> waitingFor = new List<Enemy>();

    #region Events
    public event EnemyFinishedEventHandler EnemyFinished;
    public event EnemyHitEventHandler EnemyHit;

    private void RaiseEnemyFinished()
    {
        if (EnemyFinished != null)
            EnemyFinished(this, new EnemyFinishedEventArgs(this));
    }

    private void RaiseEnemyHit(int damage)
    {
        if (EnemyHit != null)
            EnemyHit(this, new EnemyHitEventArgs(this, damage, Health));
    }
    #endregion

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

    void MoveEnemy()
    {
        float distDelta = Time.deltaTime * Speed;
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

    private void CollisionEnterProjectile(Projectile p)
    {
        Hit(p.Damage);
        p.DestroyMe();
    }

    private void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
        RaiseEnemyHit(damage);
    }

    private void Die()
    {
        Debug.Log("Dying");
        Destroy(this.gameObject);
    }

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
    /// <returns></returns>
    public float GetTotalProgress()
    {
        return pathNodeProgress + pathProgress;
    }
}

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
