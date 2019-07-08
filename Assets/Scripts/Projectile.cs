using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float TimeToLive = 0f;
    public float Damage = 1f;

    private void Update()
    {
        if (TimeToLive > 0)
        {
            TimeToLive -= Time.deltaTime;
            if (TimeToLive <= 0)
                DestroyMe();
        }
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
