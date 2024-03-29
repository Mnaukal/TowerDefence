﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class for spawning waves of enemies
/// </summary>
public class WaveController : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Called when wave starts
    /// </summary>
    public event WaveEventHandler WaveStarted;
    /// <summary>
    /// Called when all enemies of wave are dead (either killed by player or reached the end of path)
    /// </summary>
    public event WaveEventHandler WaveFinished;

    private void RaiseWaveStarted()
    {
        if (WaveStarted != null)
            WaveStarted(this, new WaveEventArgs(waveNumber, enemiesInWave)); 
    }

    private void RaiseWaveFinished()
    {
        if (WaveFinished != null)
            WaveFinished(this, new WaveEventArgs(waveNumber, enemiesInWave)); 
    }
    #endregion

    /// <summary>
    /// List of waves of enemies
    /// </summary>
    public Wave[] waves;
    /// <summary>
    /// Available types of enemies (can be further modified - speed, HP,...)
    /// </summary>
    public Enemy[] enemyTypes;

    private int waveIndex = -1;
    private int waveNumber = -1;
    private int enemiesInWave;
    private int enemiesRemaining;

    /// <summary>
    /// Start spawning of next wave
    /// </summary>
    public void SpawnNextWave()
    {
        waveIndex++;
        waveNumber++;
        if (waveIndex >= waves.Length)
        {
            if (waves[waves.Length - 1] is RepeatWave)
                waveIndex = waves.Length - 1;
            else
            {
                GameControllerS.I.Victory();
                return;
            }
        }

        waves[waveIndex].Initialize();

        enemiesInWave = waves[waveIndex].WaveItems.Sum(w => w.Count);
        enemiesRemaining = enemiesInWave;
        RaiseWaveStarted();
        StartCoroutine(SpawnWave(waveIndex));
    }

    private IEnumerator SpawnWave(int index)
    {
        Wave wave = waves[index];

        // walk through all WaveItems in Wave
        for (int i = 0; i < wave.WaveItems.Length; i++)
        {
            WaveItem w = wave.WaveItems[i];
            yield return new WaitForSeconds(w.TimeDelay); // delay before batch

            // spawn WaveItem.Count enemies
            SpawnEnemy(w);
            for (int j = 1; j < w.Count; j++)
            {
                yield return new WaitForSeconds(w.TimeBetween); // delay between enemies in batch
                SpawnEnemy(w);
            }
        }
    }

    /// <summary>
    /// Creates and sets up new enemy
    /// </summary>
    /// <param name="enemyDescription">WaveItem desribing enemy's health, speed, reward, etc.</param>
    private void SpawnEnemy(WaveItem enemyDescription)
    {
        Enemy e = Instantiate(enemyDescription.EnemyType, GameControllerS.I.EnemiesParent.transform);
        e.SetupEnemy(enemyDescription.Speed, enemyDescription.EnemyHealth, enemyDescription.Reward);
        e.EnemyFinished += GameControllerS.I.EnemyFinished;
        e.EnemyKilled += GameControllerS.I.EnemyKilledReward;
        e.EnemyFinished += EnemyFinishedOrKilled;
        e.EnemyKilled += EnemyFinishedOrKilled;
    }

    private void EnemyFinishedOrKilled(object sender, EnemyEventArgs args)
    {
        enemiesRemaining--;
        if (enemiesRemaining == 0)
            EndOfWave();
    }

    private void EndOfWave()
    {
        RaiseWaveFinished();
    }

    public void ResetGame()
    {
        waveIndex = -1;
        EndOfWave();
    }
}
