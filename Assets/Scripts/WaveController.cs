using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for spawning waves of enemies
/// </summary>
public partial class WaveController : MonoBehaviour
{
    int waveIndex = 0;

    /// <summary>
    /// Start spawning of next wave
    /// </summary>
    public void SpawnNextWave()
    {
        if (waveIndex >= waves.Length)
            return; // TODO win

        StartCoroutine(SpawnWave(waveIndex++));
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
    }
}
