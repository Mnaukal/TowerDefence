using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Projectile
{
    public Enemy DontDamage;

    public void StartGrow(float targetSize)
    {
        StartCoroutine(GrowSize(targetSize));
    }

    private IEnumerator GrowSize(float targetSize)
    {
        for (float i = 0; i <= 1; i += 1/16f)
        {
            transform.localScale = new Vector3(
                targetSize * i / transform.parent.lossyScale.x, // scale independently on parent size
                targetSize * i / transform.parent.lossyScale.y, // scale independently on parent size
                1);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Don't destroy explosion when hitting enemy
    /// </summary>
    /// <remarks>Intentionally empty function body</remarks>
    public override void ProjectileHit(Enemy enemyHit) { }

    /// <summary>
    /// Enemy hit by projectile won't be hit again by explosion
    /// </summary>
    public override int DamageEnemy(Enemy enemy)
    {
        if (enemy == DontDamage)
            return 0;

        return base.DamageEnemy(enemy);
    }
}
