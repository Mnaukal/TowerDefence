using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveController : MonoBehaviour
{
    int waveIndex = 0;

    public void SpawnNextWave()
    {
        if (waveIndex >= waves.Length)
            return; // TODO win

        StartCoroutine(SpawnWave(waveIndex++));
    }

    private IEnumerator SpawnWave(int index)
    {
        Wave wave = waves[index];

        for (int i = 0; i < wave.WaveItems.Length; i++)
        {
            WaveItem w = wave.WaveItems[i];
            yield return new WaitForSeconds(w.TimeDelay); // delay before batch

            SpawnEnemy(w);
            for (int j = 1; j < w.Count; j++)
            {
                yield return new WaitForSeconds(w.TimeBetween);
                SpawnEnemy(w);
            }
        }
    }

    private void SpawnEnemy(WaveItem enemyDescription)
    {
        Enemy e = Instantiate(enemyDescription.EnemyType, GameControllerS.I.EnemiesParent.transform);
        e.SetupEnemy(enemyDescription.Speed, enemyDescription.EnemyHealth, enemyDescription.Reward);
    }
}
