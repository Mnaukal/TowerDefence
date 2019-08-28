﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    public ShopItemTower[] towerButtons;
    [SerializeField]
    public ShopItemUpgrade[] upgradeButtons;

    [SerializeField]
    private TowerPlacer towerPlacer;
    public TowerPlacer TowerPlacer => towerPlacer;

    [SerializeField]
    private Text moneyText;

    
    [Header("TowerInfo")]
    [SerializeField]
    private Text Tower_Name;
    [SerializeField]
    private Text Tower_Damage, Tower_Range, Tower_FireRate;

    private void Start()
    {
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
        MoneyBalanceUpdated(this, new MoneyBalanceUpdatedEventArgs(GameControllerS.I.Money, 0));
        HideTowerInfoWhenTowerDeselected(this, null);
    }

    private void MoneyBalanceUpdated(object sender, MoneyBalanceUpdatedEventArgs args)
    {
        moneyText.text = "Money: " + args.NewBalance;
    }

    /// <summary>
    /// shows information about Tower in shop UI
    /// </summary>
    public void LoadTowerInfo(Tower tower)
    {
        Tower_Name.text = tower.name;
        Tower_Damage.text = "Damage: " + tower.Damage.ToString();
        Tower_Range.text = "Range: " + tower.Range.ToString();
        Tower_FireRate.text = "Fire Rate: " + (1f / tower.ReloadTime).ToString("0.##");
        Tower_Name.enabled = true;
        Tower_Damage.enabled = true;
        Tower_Range.enabled = true;
        Tower_FireRate.enabled = true;

        tower.TowerDeselected += HideTowerInfoWhenTowerDeselected;

        LoadUpgrades(tower);
    }

    public void HideTowerInfoWhenTowerDeselected(object sender, TowerEventArgs args)
    {
        Tower_Name.enabled = false;
        Tower_Damage.enabled = false;
        Tower_Range.enabled = false;
        Tower_FireRate.enabled = false;
    }

    /// <summary>
    /// Shows available upgrades for this Tower in shop UI
    /// </summary>
    public void LoadUpgrades(Tower tower)
    {
        var upgrades = GameControllerS.I.UpgradeManager.GetUpgradesForTower(tower);

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgradeButtons[i].ShowButton(upgrades[i], tower, i);
            tower.TowerDeselected += upgradeButtons[i].HideWhenTowerDeselected;
        }
    }
}
