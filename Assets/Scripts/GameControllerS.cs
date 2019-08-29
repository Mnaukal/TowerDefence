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

    [SerializeField]
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

    [SerializeField]
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
        // reload the game scene
        SceneManager.LoadScene("game");
    }
    #endregion
}
