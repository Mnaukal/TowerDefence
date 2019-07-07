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

public delegate void PointerDownEventHandler(object sender, PointerEventArgs args);
public delegate void PointerUpEventHandler(object sender, PointerEventArgs args);

public class PointerEventArgs
{

}