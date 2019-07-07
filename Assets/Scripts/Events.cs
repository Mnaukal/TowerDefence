using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEventHandler(object sender, TriggerEventArgs args);
public class TriggerEventArgs
{
    
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