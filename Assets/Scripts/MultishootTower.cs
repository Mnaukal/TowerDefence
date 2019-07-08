using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultishootTower : Tower
{
    public float fireForce = 0.05f;

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
            projectile.transform.up = directions[i];
            projectile.GetComponent<Rigidbody2D>().AddForce(directions[i] * fireForce);
        }
    }
}
