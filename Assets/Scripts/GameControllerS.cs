using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main GameController script, Singleton
/// </summary>
public class GameControllerS : MonoBehaviour
{
    // Singleton
    private static GameControllerS _instance;

    /// <summary>
    /// Singleton Instance
    /// </summary>
    public static GameControllerS I { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Links to GameObjects
    public Path Path;
    public EventManager EventManager;
    public GameObject TowersParent;
    public GameObject EnemiesParent;
    public GameObject ProjectileParent;
    public WaveController WaveController;
}
