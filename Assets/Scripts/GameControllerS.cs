using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main GameController script, Singleton
/// </summary>
public class GameControllerS : MonoBehaviour
{
    // Singleton
    private static GameControllerS instance;

    /// <summary>
    /// Singleton Instance
    /// </summary>
    public static GameControllerS I { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Links to GameObjects
    /// <summary>
    /// Path along which the enemies walk
    /// </summary>
    public Path Path;
    /// <summary>
    /// Manager of global events (mouse clicks etc.) 
    /// </summary>
    public EventManager EventManager;
    /// <summary>
    /// Parent object for all Towers (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject TowersParent;
    /// <summary>
    /// Parent object for all Enemies (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject EnemiesParent;
    /// <summary>
    /// Parent object for all Projectiles (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject ProjectileParent;
    /// <summary>
    /// Script for spawning waves of enemies
    /// </summary>
    public WaveController WaveController;
}
