using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for representing projectiles fired by towers
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// HP substracted from enemy when it gets hit by this projectile
    /// </summary>
    [HideInInspector]
    public int Damage = 1;
    /// <summary>
    /// Maximum alowed distance from tower; projectile gets destroyed when further away
    /// </summary>
    /// <remarks>setting maxDistanceSqr to 0 disables the check (also when SetMaxDistanceFromTower is not called)</remarks>
    /// <remarks>squared to increase performance (using sqrMagnitude of Vector3)</remarks>
    private float maxDistanceSqr = 0; 
    /// <summary>
    /// Position of tower to compute distance from
    /// </summary>
    private Vector3 towerPosition;

    private void Update()
    {
        if (maxDistanceSqr == 0)
            return;

        // check distance
        var distance = transform.position - towerPosition;
        if (distance.sqrMagnitude > maxDistanceSqr)
            DestroyMe();
    }

    /// <summary>
    /// Sets the position of tower and maximum allowed distance
    /// </summary>
    public void SetMaxDistanceFromTower(Vector3 towerPosition, float distance)
    {
        maxDistanceSqr = distance * distance;
        this.towerPosition = towerPosition;
    }

    /// <summary>
    /// Destroy the projectile GameObject
    /// </summary>
    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
