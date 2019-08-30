using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowing : Projectile
{
    [SerializeField]
    private Slowdown Slowdown;
    [SerializeField]
    public float slowAmount = 0.5f;
    [SerializeField]
    public float slowTime = 3f;

    public override void ProjectileHit(Enemy enemyHit)
    {
        Slowdown slowdown = Instantiate(Slowdown, enemyHit.transform);
        slowdown.SlowDownEnemy(enemyHit, slowAmount, slowTime);

        base.ProjectileHit(enemyHit);
    }
}
