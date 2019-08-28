using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    /// <summary>
    /// [towerIndex][upgradeIndex][upgradeLevel]
    /// </summary>
    private List<List<List<TowerUpgrade>>> Upgrades = new List<List<List<TowerUpgrade>>>();

    public TowerUpgrade GetUpgrade(int towerIndex, int upgradeIndex, int level)
    {
        if (towerIndex >= Upgrades.Count)
            throw new System.ArgumentOutOfRangeException(nameof(towerIndex));
        if (upgradeIndex >= Upgrades[towerIndex].Count)
            throw new System.ArgumentOutOfRangeException(nameof(upgradeIndex));
        if (level > Upgrades[towerIndex][upgradeIndex].Count)
            throw new System.ArgumentOutOfRangeException(nameof(level));

        // reached maximum level of this upgrade
        if (level == Upgrades[towerIndex][upgradeIndex].Count)
        {
            TowerUpgrade tu = Upgrades[towerIndex][upgradeIndex][level - 1];
            return new MaxTowerUpgrade(tu.Text);
        }

        return Upgrades[towerIndex][upgradeIndex][level];
    }

    public TowerUpgrade[] GetUpgradesForTower(Tower tower)
    {
        int upgradeCount = tower.UpgradeLevels.Length;
        int towerIndex = tower.TowerTypeIndex;
        TowerUpgrade[] upgrades = new TowerUpgrade[upgradeCount];

        for (int i = 0; i < upgradeCount; i++)
        {
            int currentLevel = tower.UpgradeLevels[i];
            upgrades[i] = GetUpgrade(towerIndex, i, currentLevel);
        }

        return upgrades;
    }

    public int GetUpgradeCountForTowerIndex(int towerIndex)
    {
        if (towerIndex >= Upgrades.Count)
            throw new System.ArgumentOutOfRangeException(nameof(towerIndex));

        return Upgrades[towerIndex].Count;
    }

    /// <summary>
    /// adds new Tower type to upgrade list
    /// </summary>
    public void AddTowerType()
    {
        Upgrades.Add(new List<List<TowerUpgrade>>());
    }

    /// <summary>
    /// Adds new upgrade to the Tower type
    /// </summary>
    /// <param name="towerIndex">type of Tower</param>
    /// <returns>index of newly added upgrade</returns>
    public int AddUpgrade(int towerIndex)
    {
        if (towerIndex >= Upgrades.Count)
            throw new System.ArgumentOutOfRangeException(nameof(towerIndex));

        Upgrades[towerIndex].Add(new List<TowerUpgrade>());
        return Upgrades[towerIndex].Count - 1;
    }

    /// <summary>
    /// Adds new level to upgrade with upgradeIndex of Tower with type towerIndex
    /// </summary>
    public void AddUpgradeLevel(int towerIndex, int upgradeIndex, TowerUpgrade upgradeToAdd)
    {
        if (towerIndex >= Upgrades.Count)
            throw new System.ArgumentOutOfRangeException(nameof(towerIndex));
        if (upgradeIndex >= Upgrades[towerIndex].Count)
            throw new System.ArgumentOutOfRangeException(nameof(upgradeIndex));

        Upgrades[towerIndex][upgradeIndex].Add(upgradeToAdd);
    }
}

public delegate void UpgradeFunction(Tower t);

public class TowerUpgrade
{
    public readonly int Cost;
    /// <summary>
    /// Function to be called when upgrading, set to null to disable upgrade (for example when max level is reached)
    /// </summary>
    public readonly UpgradeFunction Function;
    public readonly string Text, Level;

    public TowerUpgrade(int cost, UpgradeFunction function, string text, string level)
    {
        Cost = cost;
        Function = function;
        Text = text;
        Level = level;
    }
}

/// <summary>
/// Indicates that this is the maximum level of this type of upgrade
/// </summary>
public class MaxTowerUpgrade : TowerUpgrade
{
    public MaxTowerUpgrade(string text) : base(0, null, text, "max") { }
}

public class BasicUpgrades
{
    public static UpgradeFunction Damage(int newValue)
    {
        return t => { t.Damage = newValue; };
    }

    public static UpgradeFunction Range(float newValue)
    {
        return t => { t.Range = newValue; };
    }

    public static UpgradeFunction ReloadTime(float newValue)
    {
        return t => { t.ReloadTime = newValue; };
    }
}
