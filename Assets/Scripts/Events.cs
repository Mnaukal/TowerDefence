using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEnterEventHandler(object sender, TriggerEventArgs args);
public delegate void TriggerExitEventHandler(object sender, TriggerEventArgs args);

public class TriggerEventArgs
{
    public Collider2D Collision { get; }

    public TriggerEventArgs(Collider2D collision)
    {
        Collision = collision;
    }
}

public delegate void EnemyFinishedEventHandler(object sender, EnemyFinishedEventArgs args);
public class EnemyFinishedEventArgs
{
    public Enemy enemy { get; }

    public EnemyFinishedEventArgs(Enemy enemy)
    {
        this.enemy = enemy;
    }
}
public delegate void EnemyHitEventHandler(object sender, EnemyHitEventArgs args);
public class EnemyHitEventArgs
{
    public Enemy enemy { get; }
    /// <summary>
    /// Damage given in this hit
    /// </summary>
    public int Damage { get; }
    /// <summary>
    /// Remaining health of the enemy
    /// </summary>
    public int Health { get; }

    public EnemyHitEventArgs(Enemy enemy, int damage, int health)
    {
        this.enemy = enemy;
        Damage = damage;
        Health = health;
    }
}

public delegate void PointerDownEventHandler(object sender, PointerEventArgs args);
public delegate void PointerUpEventHandler(object sender, PointerEventArgs args);

public class PointerEventArgs
{

}