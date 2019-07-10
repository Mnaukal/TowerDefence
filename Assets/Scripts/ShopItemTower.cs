using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTower : MonoBehaviour
{
    [Header("Tower Parameters")]
    [SerializeField]
    private Tower Tower;
    [SerializeField]
    private int Cost;
    [SerializeField]
    private string TowerName;
    [Header("UI objects links")]
    [SerializeField]
    private Text Text_Name;
    [SerializeField]
    private Text Text_Cost, Text_Damage, Text_Range, Text_FireRate;

    private void Start()
    {
        // Setup UI
        Text_Name.text = TowerName;
        Text_Cost.text = Cost.ToString();
        Text_Damage.text = "Damage: " + Tower.GetDamage().ToString();
        Text_Range.text = "Range: " + Tower.GetRange().ToString();
        Text_FireRate.text = "Fire Rate: " + (1f / Tower.GetReloadTime()).ToString("0.##");

        // Link event
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
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
        GameControllerS.I.SubtractMoney(Cost);
        TowerPlacer tp = Instantiate(GameControllerS.I.Shop.TowerPlacer);
        tp.Tower = Tower;
    }
}
