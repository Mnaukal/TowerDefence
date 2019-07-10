using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tower which shoots multiple projectiles at the same time to different directions
/// </summary>
public class MultishootTower : Tower
{
    // 8 directions of shooting
    private Vector2[] directions = new Vector2[] {
        new Vector2(1, 0),
        new Vector2(1, 1).normalized,
        new Vector2(0, 1),
        new Vector2(1, -1).normalized,
        new Vector2(-1, 0),
        new Vector2(-1, -1).normalized,
        new Vector2(0, -1),
        new Vector2(-1, 1).normalized,
    };

    protected override void Shoot()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            var projectile = Instantiate(Projectile, transform.position, Quaternion.identity, GameControllerS.I.ProjectileParent.transform);
            // set direction and speed of projectile
            projectile.transform.up = directions[i];
            projectile.GetComponent<Rigidbody2D>().AddForce(directions[i] * fireForce);
            // set parameters
            projectile.SetMaxDistanceFromTower(transform.position, Range);
            projectile.Damage = Damage;
        }
    }
}
