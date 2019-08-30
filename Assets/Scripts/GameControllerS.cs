using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    #region Links to GameObjects

    // [SerializeField] exposes the field in Unity Editor Inspector in order to connect the reference

    [SerializeField]
    private Path path;
    /// <summary>
    /// Path along which the enemies walk
    /// </summary>
    public Path Path => path;

    [SerializeField]
    private EventManager eventManager;
    /// <summary>
    /// Manager of global events (mouse clicks etc.) 
    /// </summary>
    public EventManager EventManager => eventManager;

    [SerializeField]
    private GameObject towersParent;
    /// <summary>
    /// Parent object for all Towers (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject TowersParent => towersParent;

    [SerializeField]
    private GameObject enemiesParent;
    /// <summary>
    /// Parent object for all Enemies (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject EnemiesParent => enemiesParent;

    [SerializeField]
    private GameObject projectileParent;
    /// <summary>
    /// Parent object for all Projectiles (to keep the hierarchy in Unity well-aranged)
    /// </summary>
    public GameObject ProjectileParent => projectileParent;

    [SerializeField]
    private WaveController waveController;
    /// <summary>
    /// Script for spawning waves of enemies
    /// </summary>
    public WaveController WaveController => waveController;

    [SerializeField]
    private Shop shop;
    /// <summary>
    /// Buying towers and upgrades
    /// </summary>
    public Shop Shop => shop;

    [SerializeField]
    private UpgradeManager upgradeManager;
    /// <summary>
    /// List of possible upgrades for all towers
    /// </summary>
    public UpgradeManager UpgradeManager => upgradeManager;

    [SerializeField]
    private GameObject gameOverScreen;
    /// <summary>
    /// Path along which the enemies walk
    /// </summary>
    public GameObject GameOverScreen => gameOverScreen;

    [SerializeField]
    private GameObject victoryScreen;
    /// <summary>
    /// Path along which the enemies walk
    /// </summary>
    public GameObject VictoryScreen => victoryScreen;
    #endregion

    #region Money
    #region events
    /// <summary>
    /// Raised when money is added or subtracted
    /// </summary>
    public event MoneyBalanceUpdatedHandler MoneyBalanceUpdated;

    private void RaiseMoneyBalanceUpdated(int newBalance, int oldBalance)
    {
        if (MoneyBalanceUpdated != null)
            MoneyBalanceUpdated(this, new MoneyBalanceUpdatedEventArgs(newBalance, oldBalance));
    }
    #endregion

    private int money = 0;
    public int Money
    {
        get
        {
            return money;
        }
        private set
        {
            int oldValue = money;
            money = value;
            RaiseMoneyBalanceUpdated(money, oldValue);
        }
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
            throw new System.ArgumentException("Adding negative amount not allowed, use SubtractMoney.", nameof(amount));

        Money += amount;
    }

    public void SubtractMoney(int amount)
    {
        if (amount < 0)
            throw new System.ArgumentException("Subtracting negative amount not allowed, use AddMoney.", nameof(amount));

        Money -= amount;
    }

    #endregion
    #region Lives
    #region events
    /// <summary>
    /// Raised when money is added or subtracted
    /// </summary>
    public event LivesUpdatedHandler LivesUpdated;

    private void RaiseLivesUpdated(int newLives, int oldLives)
    {
        if (LivesUpdated != null)
            LivesUpdated(this, new LivesUpdatedEventArgs(newLives, oldLives));
    }
    #endregion

    private int lives = 0;
    public int Lives
    {
        get
        {
            return lives;
        }
        private set
        {
            int oldValue = lives;
            lives = value;
            RaiseLivesUpdated(lives, oldValue);
        }
    }

    public void EnemyFinished(object sender, EnemyEventArgs args)
    {
        Debug.Log("Enemy finished: " + args.enemy);

        Lives--;
        if (Lives <= 0)
            GameOver();
    }

    public void EnemyKilledReward(object sender, EnemyEventArgs args)
    {
        Debug.Log("Enemy killed: " + args.enemy);
        AddMoney(args.enemy.Reward);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("Starting game...");
        Time.timeScale = 1;
        VictoryScreen.SetActive(false);
        GameOverScreen.SetActive(false);

        Money = GetComponent<ConfiguraionLoader>().DefaultMoney;
        Lives = GetComponent<ConfiguraionLoader>().DefaultLives;

        foreach (Transform child in EnemiesParent.transform)
            GameObject.Destroy(child.gameObject);
        foreach (Transform child in ProjectileParent.transform)
            GameObject.Destroy(child.gameObject);
        foreach (Transform child in TowersParent.transform)
            GameObject.Destroy(child.gameObject);

        GetComponent<WaveController>().ResetGame();
    }

    public void Victory()
    {
        Debug.Log("Victory!");
        Time.timeScale = 0;
        VictoryScreen.SetActive(true);
    }
    #endregion
}
