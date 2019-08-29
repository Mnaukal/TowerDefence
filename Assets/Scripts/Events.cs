using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file describes all types of events used in the game

#region Trigger collider events
public delegate void TriggerEventHandler(object sender, TriggerEventArgs args);
public class TriggerEventArgs
{
    public Collider2D Collision { get; }

    public TriggerEventArgs(Collider2D collision)
    {
        Collision = collision;
    }
}
#endregion

#region Enemy events
public delegate void EnemyEventHandler(object sender, EnemyEventArgs args);
public class EnemyEventArgs
{
    public Enemy enemy { get; }

    public EnemyEventArgs(Enemy enemy)
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

    public Vector2 HitPosition { get; }

    public EnemyHitEventArgs(Enemy enemy, int damage, int health, Vector2 hitPosition)
    {
        this.enemy = enemy;
        Damage = damage;
        Health = health;
        HitPosition = hitPosition;
    }
}
#endregion

#region Tower events 
public delegate void TowerEventHandler(object sender, TowerEventArgs args);
public class TowerEventArgs
{
    public Tower tower { get; }

    public TowerEventArgs(Tower tower)
    {
        this.tower = tower;
    }
}

public delegate void TowerShotEventHandler(object sender, TowerShotEventArgs args);
public class TowerShotEventArgs
{
    public Tower Tower { get; }
    public Projectile Projectile { get; }

    public TowerShotEventArgs(Tower tower, Projectile projectile)
    {
        Tower = tower;
        Projectile = projectile;
    }
}

#endregion

#region Game events 
public delegate void MoneyBalanceUpdatedHandler(object sender, MoneyBalanceUpdatedEventArgs args);
public class MoneyBalanceUpdatedEventArgs
{
    public int NewBalance { get; }
    public int OldBalance { get; }

    public MoneyBalanceUpdatedEventArgs(int newBalance, int oldBalance)
    {
        NewBalance = newBalance;
        OldBalance = oldBalance;
    }
}

public delegate void LivesUpdatedHandler(object sender, LivesUpdatedEventArgs args);
public class LivesUpdatedEventArgs
{
    public int NewLives { get; }
    public int OldLives { get; }

    public LivesUpdatedEventArgs(int newLives, int oldLives)
    {
        NewLives = newLives;
        OldLives = oldLives;
    }
}

public delegate void WaveEventHandler(object sender, WaveEventArgs args);
public class WaveEventArgs
{
    public int WaveNumber { get; }
    public int NumberOfEnemies { get; }

    public WaveEventArgs(int waveNumber, int numberOfEnemies)
    {
        WaveNumber = waveNumber;
        NumberOfEnemies = numberOfEnemies;
    }
}
#endregion

#region Global events
public delegate void PointerEventHandler(object sender, PointerEventArgs args);
public class PointerEventArgs
{
    public Vector3 mouseScreenPosition { get; }
    public Vector3 mouseWolrdPosition {get; }

    public PointerEventArgs(Vector3 mouseScreenPosition, Vector3 mouseWolrdPosition)
    {
        this.mouseScreenPosition = mouseScreenPosition;
        this.mouseWolrdPosition = mouseWolrdPosition;
    }
}
#endregion