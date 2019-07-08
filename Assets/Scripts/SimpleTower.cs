using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : Tower
{
    public float fireForce = 1f;

    protected override void Shoot()
    {
        var targetEnemy = FirstEnemyInRange();
        var projectile = Instantiate(Projectile, transform.position, Quaternion.identity, GameControllerS.I.ProjectileParent.transform);
        Vector2 targetDirection = (targetEnemy.transform.position - transform.position).normalized;
        projectile.transform.up = targetDirection;
        projectile.GetComponent<Rigidbody2D>().AddForce(targetDirection * fireForce);
    }
}
