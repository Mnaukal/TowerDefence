using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WaveController : MonoBehaviour
{
    public Enemy[] enemyTypes;

    private Wave[] waves;

    // WaveItem(timeDelay, timeBetween, enemyType, enemyHealth, reward, speed, [count])

    private void Awake()
    {
        waves = new Wave[]
        {
            new Wave(new WaveItem[] {
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f, 3),
                    new WaveItem(5f, 2f, enemyTypes[0], 10, 1, 1.4f, 3),
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f, 3)
                }),
            new Wave(new WaveItem[] {
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f),
                    new WaveItem(5f, 2f, enemyTypes[0], 10, 1, 1.4f),
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f)
                }),
            new Wave(new WaveItem[] {
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f, 3),
                    new WaveItem(5f, 2f, enemyTypes[0], 10, 1, 1.4f, 3),
                    new WaveItem(1f, 1f, enemyTypes[0], 5, 1, 1f, 3)
                }),
        };
    }
}
