using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToLive : MonoBehaviour
{
    private float timeRemaining = 0f;

    public void SetTime(float time)
    {
        timeRemaining = time;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
                DestroyMe();
        }
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
