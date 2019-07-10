using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Tower which shoots at random enemy in range
/// </summary>
public class RandomTargetTower : SimpleTower
{
    /// <summary>
    /// Overrides this function to return random enemy in range instead of the first one
    /// </summary>
    protected override Enemy FirstEnemyInRange()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("Enemy")).Select(x => x.GetComponent<Enemy>()).ToArray();
        if (enemies == null || enemies.Length == 0)
            return null;
        return enemies[Random.Range(0, enemies.Length)];
    }
}
