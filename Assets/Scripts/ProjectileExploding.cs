using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This projectile creates explosion which damages other enemies in certain range
/// </summary>
public class ProjectileExploding : Projectile
{
    [SerializeField]
    private Explosion Explosion;
    [SerializeField]
    private float explosionSize = 2f;

    public override void ProjectileHit(Enemy enemyHit)
    {
        Explosion.Damage = Damage;
        Explosion.DontDamage = enemyHit;
        Explosion.transform.parent = GameControllerS.I.ProjectileParent.transform;
        Explosion.gameObject.SetActive(true);
        Explosion.StartGrow(explosionSize);
        
        base.ProjectileHit(enemyHit);
    }
}
