using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents one button in shop (for buying one type of upgrade)
/// </summary>
public class ShopItemUpgrade : MonoBehaviour
{
    [Header("UI objects links")]
    [SerializeField]
    private Text Text_Name;
    [SerializeField]
    private Text Text_Cost, Text_Level;

    TowerUpgrade Upgrade;   // upgrade to perform
    Tower TargetTower;      // tower which gets upgraded
    int Cost;
    int UpgradeIndex;       // index of the upgrade on this tower

    private void Start()
    {
        // Setup UI
        ClearUI();

        // Link event
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
    }

    /// <summary>
    /// Hides button in shop.
    /// </summary>
    public void ClearUI()
    {
        Text_Name.text = "";
        Text_Cost.text = "";
        HideButton();
    }

    public void ShowButton(TowerUpgrade upgrade, Tower targetTower, int upgradeIndex)
    {
        Text_Name.text = upgrade.Text;
        Text_Level.text = upgrade.Level;
        Upgrade = upgrade;
        TargetTower = targetTower;
        UpgradeIndex = upgradeIndex;

        if (upgrade is MaxTowerUpgrade)
            Text_Cost.text = "";
        else
            Text_Cost.text = upgrade.Cost.ToString();

        Cost = upgrade.Cost;
        OnEnable();

        gameObject.SetActive(true);
    }

    private void HideButton()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Hide upgrade buttons when Tower is deselected. Hook this to TowerDeselected event of Tower
    /// </summary>
    public void HideWhenTowerDeselected(object sender, TowerEventArgs args)
    {
        ClearUI();
    }

    private void OnEnable() // Unity method which gets called when GameObject is enabled
    {
        MoneyBalanceUpdated(this, new MoneyBalanceUpdatedEventArgs(GameControllerS.I.Money, GameControllerS.I.Money));
    }

    private void MoneyBalanceUpdated(object sender, MoneyBalanceUpdatedEventArgs args)
    {
        if (args.NewBalance >= Cost && !(Upgrade is MaxTowerUpgrade))
            EnableBuy();
        else
            DisableBuy();
    }

    private void EnableBuy()
    {
        GetComponent<Button>().interactable = true;
    }

    private void DisableBuy()
    {
        GetComponent<Button>().interactable = false;
    }

    public void BuyUpgrade()
    {
        if (GameControllerS.I.Money < Cost)
            throw new System.InvalidOperationException("This method should only be called when clicking the button in shop (which should be only enabled if user has enough money).");

        GameControllerS.I.SubtractMoney(Cost);
        Upgrade.Function(TargetTower);
        TargetTower.UpgradeLevels[UpgradeIndex]++;
        GameControllerS.I.Shop.LoadTowerInfo(TargetTower);
    }
}
