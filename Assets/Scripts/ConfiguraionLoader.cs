using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Parse wave and tower configuration from text file
/// </summary>
public class ConfiguraionLoader : MonoBehaviour
{
    [SerializeField]
    private ShopItemTower[] shopItems;
    [SerializeField]
    private WaveUI waveUI;

    void Start()
    {
        string waveConfigFile = System.IO.Path.Combine(Application.streamingAssetsPath, "WaveConfiguration.txt");
        string towerConfigFile = System.IO.Path.Combine(Application.streamingAssetsPath, "TowerConfiguration.txt");

        try
        {
            LoadTowerConfing(towerConfigFile);
            LoadWaveConfing(waveConfigFile);
        }
        catch (ConfigFileException e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void LoadTowerConfing(string towerConfigFile)
    {
        int lineNumber = 0;
        int towerIndex = 0;
        using (StreamReader sr = new StreamReader(towerConfigFile))
        {
            while (true)
            {
                lineNumber++;
                string line = sr.ReadLine();
                if (line == null)
                    break;

                if (line.Length == 0 || line[0] == '#') // comment in file
                    continue;

                string[] tokens = line.Split(',');

                ParseTower(tokens, towerIndex, lineNumber);
                shopItems[towerIndex].SetupShopUI();
                towerIndex++;
            }
        }

    }

    private void ParseTower(string[] tokens, int towerIndex, int lineNumber)
    {
        if (tokens.Length != 5)
            throw new ConfigFileException("Invalid number of values on line " + lineNumber);

        shopItems[towerIndex].TowerName = tokens[0];

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

    private void LoadWaveConfing(string waveConfigFile)
    {
        int lineNumber = 0;
        List<Wave> waves = new List<Wave>();
        List<WaveItem> items = new List<WaveItem>();

        using (StreamReader sr = new StreamReader(waveConfigFile))
        {
            while (true)
            {
                lineNumber++;
                string line = sr.ReadLine();
                if (line == null)
                    break;

                if (line.Length == 0 || line[0] == '#') // comment in file
                    continue;

                string[] tokens = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if(tokens.Length > 0 && tokens[0] == "wave") // add current wave to wave list
                {
                    if (items.Count > 0)
                    {
                        waves.Add(new Wave(items.ToArray()));
                    }
                    items.Clear();
                    continue;
                }

                WaveItem item = ParseWaveItem(tokens, lineNumber);
                items.Add(item);
            }

            // add last wave
            if (items.Count > 0) 
                waves.Add(new Wave(items.ToArray()));
           
        }

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
