using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component to destroy GameObject after set amount of time
/// </summary>
/// <remarks>if time is not set by SetTime, this script is not active</remarks>
public class TimeToLive : MonoBehaviour
{
    private float timeRemaining = 0f;

    /// <summary>
    /// Sets the time to wait before destroying GameObject and starts the countdown
    /// </summary>
    /// <param name="time">time in seconds</param>
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
