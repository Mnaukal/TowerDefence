using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slows down an Enemy
/// </summary>
public class Slowdown : MonoBehaviour
{
    private Enemy Enemy;
    private float originalSpeed;
    private float timer;

    /// <summary>
    /// Slows down an enemy
    /// </summary>
    /// <param name="enemy">Enemy to slow down</param>
    /// <param name="slowAmount">Fraction between 0 and 1 which determines new speed of the enemy relative to its original speed</param>
    public void SlowDownEnemy(Enemy enemy, float slowAmount, float slowTime)
    {
        if (slowAmount < 0 || slowAmount > 1)
            throw new System.ArgumentException("slowAmount must be between 0 and 1", nameof(slowAmount));

        foreach (Slowdown s in enemy.GetComponentsInChildren<Slowdown>())
        {
            if (s != this)
                s.ResetSpeed();
        }

        Enemy = enemy;
        originalSpeed = enemy.Speed;
        enemy.Speed *= slowAmount;
        timer = slowTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            ResetSpeed();
    }

    private void ResetSpeed()
    {
        Enemy.Speed = originalSpeed;
        Destroy(this.gameObject);
    }
}
