using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int Damage = 1;
    private float maxDistanceSqr; // squared to increase performance (sqrMagnitude of Vector3)
    private Vector3 towerPosition;

    private void Update()
    {
        var distance = transform.position - towerPosition;
        if (distance.sqrMagnitude > maxDistanceSqr)
            DestroyMe();
    }

    public void SetMaxDistanceFromTower(Vector3 towerPosition, float distance)
    {
        maxDistanceSqr = distance * distance;
        this.towerPosition = towerPosition;
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
