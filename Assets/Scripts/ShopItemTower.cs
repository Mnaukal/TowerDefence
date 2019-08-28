using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTower : MonoBehaviour
{
    [Header("Tower Parameters")]
    public Tower Tower;
    public int Cost;
    public string TowerName;
    [Header("UI objects links")]
    [SerializeField]
    private Text Text_Name;
    [SerializeField]
    private Text Text_Cost, Text_Damage, Text_Range, Text_FireRate;

    private void Start()
    {
        // Setup UI
        SetupShopUI();

        // Link event
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
    }

    public void SetupShopUI()
    {
        Text_Name.text = TowerName;
        Text_Cost.text = Cost.ToString();
        Text_Damage.text = "Damage: " + Tower.Damage.ToString();
        Text_Range.text = "Range: " + Tower.Range.ToString();
        Text_FireRate.text = "Fire Rate: " + (1f / Tower.ReloadTime).ToString("0.##");
    }

    private void MoneyBalanceUpdated(object sender, MoneyBalanceUpdatedEventArgs args)
    {
        if (args.NewBalance >= Cost)
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

    public void BuyTower()
    {
        if (GameControllerS.I.Money < Cost)
            throw new System.InvalidOperationException("This method should only be called when clicking the button in shop (which should be only enabled if user has enough money).");

        GameControllerS.I.SubtractMoney(Cost);
        TowerPlacer tp = Instantiate(GameControllerS.I.Shop.TowerPlacer);
        tp.Tower = Tower;
    }
}
