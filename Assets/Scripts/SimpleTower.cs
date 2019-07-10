using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower which shoots at first (with highest progress along path) enemy in range
/// </summary>
public class SimpleTower : Tower
{
    /// <summary>
    /// Implements shooting of this tower
    /// </summary>
    protected override void Shoot()
    {
        var targetEnemy = FirstEnemyInRange();
        if (targetEnemy == null)
            return;
        // create projectile
        var projectile = Instantiate(Projectile, transform.position, Quaternion.identity, GameControllerS.I.ProjectileParent.transform);
        // set direction and speed of projectile
        Vector2 targetDirection = (targetEnemy.transform.position - transform.position).normalized;
        projectile.transform.up = targetDirection;
        projectile.GetComponent<Rigidbody2D>().AddForce(targetDirection * fireForce);
        // set parameters
        projectile.SetMaxDistanceFromTower(transform.position, Range);
        projectile.Damage = Damage;
    }
}
