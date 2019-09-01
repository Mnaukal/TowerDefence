using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for controlling the UI of shop
/// </summary>
public class Shop : MonoBehaviour
{
    [SerializeField]
    public ShopItemTower[] towerButtons;
    [SerializeField]
    public ShopItemUpgrade[] upgradeButtons;

    [SerializeField]
    private TowerPlacer towerPlacer;
    /// <summary>
    /// Reference to TowerPlacer Prefab
    /// </summary>
    public TowerPlacer TowerPlacer => towerPlacer;
    /// <summary>
    /// Used to disable right panel buttons when placing Tower
    /// </summary>
    [SerializeField]
    private GameObject rightPanelOverlay;
    
    [Header("TowerInfo")] // for displaying information about selected Tower
    [SerializeField]
    private Text Tower_Name;
    [SerializeField]
    private Text Tower_Damage, Tower_Range, Tower_FireRate;

    private void Start()
    {
        HideTowerInfoWhenTowerDeselected(this, null);
    }

    /// <summary>
    /// Shows information about Tower in shop UI. Should be called when Tower gets selected.
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

    /// <summary>
    /// Clears Tower info UI objects. Should be hooked to TowerDeselected event of Tower.
    /// </summary>
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

    /// <summary>
    /// Shows the overlay over the right panel, so it isn't clickable
    /// </summary>
    public void DisableRightPanel()
    {
        rightPanelOverlay.SetActive(true);
    }

    /// <summary>
    /// Disables the overlay
    /// </summary>
    public void EnableRightPanel()
    {
        rightPanelOverlay.SetActive(false);
    }
}
