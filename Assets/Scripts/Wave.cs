using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for describing one wave of enemies
/// </summary>
public class Wave
{
    public WaveItem[] WaveItems;

    public Wave(WaveItem[] waveItems)
    {
        WaveItems = waveItems;
    }
}

/// <summary>
/// Class for describing one set of enemies of same time spawned in row
/// Wave contains a list of WaveItems and each WaveItem represents one or more enemies to be spawned (all of same type and properties)
/// </summary>
public class WaveItem
{
    /// <summary>
    /// Seconds before first enemy of this batch will be spawned, relative to last spawned batch (WaveItem)
    /// </summary>
    public float TimeDelay;

    /// <summary>
    /// Seconds of delay between spawning enemies of this batch (WaveItem)
    /// </summary>
    public float TimeBetween;

    /// <summary>
    /// Type of enemy spawned in this batch
    /// </summary>
    public Enemy EnemyType;

    /// <summary>
    /// HP every enemy in this batch will have
    /// </summary>
    public int EnemyHealth;

    /// <summary>
    /// Reward money given for killing each enemy in batch
    /// </summary>
    public int Reward;

    /// <summary>
    /// Speed of every enemy in batch
    /// </summary>
    public float Speed;

    /// <summary>
    /// Number of enemies spawned in this batch
    /// </summary>
    public int Count;

    /// <summary>
    /// Creates new WaveItem
    /// </summary>
    /// <param name="timeDelay">Seconds before first enemy of this batch will be spawned, relative to last spawned batch (WaveItem)</param>
    /// <param name="timeBetween">Seconds of delay between spawning enemies of this batch (WaveItem)</param>
    /// <param name="enemyType">Type of enemy spawned in this batch</param>
    /// <param name="enemyHealth">HP every enemy in this batch will have</param>
    /// <param name="reward">Reward money given for killing each enemy in batch</param>
    /// <param name="speed">Speed of every enemy in batch</param>
    /// <param name="count">Number of enemies spawned in this batch</param>
    public WaveItem(float timeDelay, float timeBetween, Enemy enemyType, int enemyHealth, int reward, float speed, int count = 1)
    {
        if(timeDelay < 0)
            throw new System.ArgumentException("Time can't be negative.", nameof(timeDelay));
        TimeDelay = timeDelay;

        if(timeBetween < 0)
            throw new System.ArgumentException("Time can't be negative.", nameof(timeBetween));
        TimeBetween = timeBetween;

        if (enemyType == null)
            throw new System.ArgumentNullException(nameof(enemyType));
        EnemyType = enemyType;

        if (enemyHealth <= 0)
            throw new System.ArgumentException("Health must be positive.", nameof(enemyHealth));
        EnemyHealth = enemyHealth;

        if (reward < 0)
            throw new System.ArgumentException("Reward can't be negative.", nameof(reward));
        Reward = reward;

        if(speed <= 0)
            throw new System.ArgumentException("Speed must be positive.", nameof(speed));
        Speed = speed;

        if (count <= 0)
            throw new System.ArgumentException("WaveItem must spawn at least one enemy.", nameof(count));
        Count = count;
    }
}
