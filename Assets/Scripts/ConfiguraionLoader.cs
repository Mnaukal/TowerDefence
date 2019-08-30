using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

/// <summary>
/// Parse wave and tower configuration from text file
/// </summary>
public class ConfiguraionLoader : MonoBehaviour
{
    [SerializeField]
    private WaveUI waveUI;
    private ShopItemTower[] shopItems;
    public int DefaultMoney
    {
        get;
        private set;
    }
    public int DefaultLives
    {
        get;
        private set;
    }

    void Start()
    {
        shopItems = GameControllerS.I.Shop.towerButtons;
        try
        {
            StartCoroutine(LoadStreamingAsset("TowerConfiguration.txt", LoadTowerConfing));
            StartCoroutine(LoadStreamingAsset("WaveConfiguration.txt", LoadWaveConfing));
        }
        catch (ConfigFileException e)
        {
            Debug.LogError(e.Message);
        }
    }

    IEnumerator LoadStreamingAsset(string filename, System.Action<string> parser)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, filename);

        string result = "";

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                result = www.downloadHandler.text;
            }
        }
        else
            result = File.ReadAllText(filePath);

        parser(result);
    }

    private void LoadTowerConfing(string towerConfigFile)
    {
        string[] lines = towerConfigFile.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("Loading Tower Configuration: " + lines.Length + " lines.");
        int towerIndex = -1; // increased by 1 before adding first tower

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber];
            if (line == null)
                break;

            if (line.Length == 0 || line[0] == '#') // comment in file
                continue;

            string[] tokens = line.Split(',');
            if(tokens.Length == 0)
                throw new ConfigFileException("Invalid number of values on line " + (lineNumber + 1));

            if(tokens[0] == "d") // defaults
            {
                ParseDefaults(tokens, lineNumber + 1);
            }
            else if (tokens[0] == "u") // upgrade
            {
                ParseUpgrade(tokens, towerIndex, lineNumber + 1);
            }
            else
            {
                towerIndex++;
                ParseTower(tokens, towerIndex, lineNumber + 1);
                shopItems[towerIndex].SetupShopUI();
                GameControllerS.I.UpgradeManager.AddTowerType();
            }
        }

        GameControllerS.I.RestartGame();
    }

    private void ParseTower(string[] tokens, int towerIndex, int lineNumber)
    {
        if (tokens.Length != 5)
            throw new ConfigFileException("Invalid number of values on line " + lineNumber);

        shopItems[towerIndex].TowerName = tokens[0];
        shopItems[towerIndex].Tower.TowerTypeIndex = towerIndex;

        if (!int.TryParse(tokens[1], out int cost))
            throw new ConfigFileException("Invalid tower cost on line " + lineNumber);
        shopItems[towerIndex].Cost = cost;

        if (!int.TryParse(tokens[2], out int damage))
            throw new ConfigFileException("Invalid tower damage on line " + lineNumber);
        shopItems[towerIndex].Tower.Damage = damage;

        if (!float.TryParse(tokens[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float range))
            throw new ConfigFileException("Invalid tower range on line " + lineNumber);
        shopItems[towerIndex].Tower.Range = range;

        if (!float.TryParse(tokens[4], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float reload))
            throw new ConfigFileException("Invalid tower reload time on line " + lineNumber);
        shopItems[towerIndex].Tower.ReloadTime = reload;
    }

    private void ParseUpgrade(string[] tokens, int towerIndex, int lineNumber)
    {
        if (tokens.Length < 3)
            throw new ConfigFileException("Invalid number of values on line " + lineNumber);

        int upgradeIndex = GameControllerS.I.UpgradeManager.AddUpgrade(towerIndex);
        for (int i = 2; i < tokens.Length; i++)
        {
            string[] tokensUpgrade = tokens[i].Split(':');
            var up = ParseUpgradeLevel(tokensUpgrade, upgradeIndex, tokens[1], "Level " + (i - 1).ToString(), lineNumber);
            GameControllerS.I.UpgradeManager.AddUpgradeLevel(towerIndex, upgradeIndex, up);
        }
    }

    private TowerUpgrade ParseUpgradeLevel(string[] tokens, int upgradeIndex, string name, string level, int lineNumber)
    {
        if (tokens.Length != 3)
            throw new ConfigFileException("Invalid number of values in upgrade level on line " + lineNumber);

        if (!int.TryParse(tokens[0], out int cost))
            throw new ConfigFileException("Invalid upgrade cost on line " + lineNumber);

        switch (tokens[1])
        {
            case "d":
                if (!int.TryParse(tokens[2], out int damage))
                    throw new ConfigFileException("Invalid upgrade damage on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.Damage(damage), name + " " + damage, level);

            case "r":
                if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float range))
                    throw new ConfigFileException("Invalid tower range on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.Range(range), name + " " + range, level);

            case "t":
                if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float time))
                    throw new ConfigFileException("Invalid tower range on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.ReloadTime(time), name + " " + time, level);

            case "e":
                if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float explosionSize))
                    throw new ConfigFileException("Invalid explosion size on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.ExplosionSize(explosionSize), name + " " + explosionSize, level);

            case "sa":
                if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float slowAmount))
                    throw new ConfigFileException("Invalid slow amount on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.SlowAmount(slowAmount), name + " " + slowAmount, level);

            case "sd":
                if (!float.TryParse(tokens[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float slowDuration))
                    throw new ConfigFileException("Invalid slow duration on line " + lineNumber);
                return new TowerUpgrade(cost, BasicUpgrades.SlowDuration(slowDuration), name + " " + slowDuration, level);

            default:
                throw new ConfigFileException("Invalid upgrade type on line " + lineNumber);
        }
    }

    private void ParseDefaults(string[] tokens, int lineNumber)
    {
        if (tokens.Length != 3)
            throw new ConfigFileException("Invalid number of values on line " + lineNumber);

        if (!int.TryParse(tokens[1], out int money))
            throw new ConfigFileException("Invalid default money on line " + lineNumber);
        if (!int.TryParse(tokens[2], out int lives))
            throw new ConfigFileException("Invalid default lives on line " + lineNumber);

        DefaultMoney = money;
        DefaultLives = lives;
    }

    private void LoadWaveConfing(string waveConfigFile)
    {
        string[] lines = waveConfigFile.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("Loading Wave Configuration: " + lines.Length + " lines.");

        List<Wave> waves = new List<Wave>();
        List<WaveItem> items = new List<WaveItem>();

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber];
            if (line == null)
                break;

            if (line.Length == 0 || line[0] == '#') // comment in file
                continue;

            string[] tokens = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length > 0 && tokens[0] == "wave") // add current wave to wave list
            {
                if (items.Count > 0)
                {
                    waves.Add(new Wave(items.ToArray()));
                }
                items.Clear();
                continue;
            }

            WaveItem item = ParseWaveItem(tokens, lineNumber + 1);
            items.Add(item);
        }

        // add last wave
        if (items.Count > 0)
            waves.Add(new Wave(items.ToArray()));

        GameControllerS.I.WaveController.waves = waves.ToArray();
        waveUI.EnableStartButton();
    }

    private WaveItem ParseWaveItem(string[] tokens, int lineNumber)
    {
        if (tokens.Length != 6 && tokens.Length != 7)
            throw new ConfigFileException("Invalid number of values on line " + lineNumber);

        if (!float.TryParse(tokens[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float timeDelay))
            throw new ConfigFileException("Invalid time delay on line " + lineNumber);

        if (!float.TryParse(tokens[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float timeBetween))
            throw new ConfigFileException("Invalid time between on line " + lineNumber);

        if (!int.TryParse(tokens[2], out int enemyIndex) || enemyIndex >= GameControllerS.I.WaveController.enemyTypes.Length)
            throw new ConfigFileException("Invalid enemy index on line " + lineNumber);

        if (!int.TryParse(tokens[3], out int enemyHealth))
            throw new ConfigFileException("Invalid enemy health on line " + lineNumber);

        if (!int.TryParse(tokens[4], out int reward))
            throw new ConfigFileException("Invalid enemy reward on line " + lineNumber);

        if (!float.TryParse(tokens[5], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float speed))
            throw new ConfigFileException("Invalid enemy speed on line " + lineNumber);

        int count = 1;
        if(tokens.Length == 7)
        {
            if (!int.TryParse(tokens[6], out count))
                throw new ConfigFileException("Invalid enemy count on line " + lineNumber);
        }

        return new WaveItem(timeDelay, timeBetween, GameControllerS.I.WaveController.enemyTypes[enemyIndex], enemyHealth, reward, speed, count);
    }
}

class ConfigFileException : System.Exception
{
    public ConfigFileException(string message) : base(message) { }
}
