using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private TowerPlacer towerPlacer;
    public TowerPlacer TowerPlacer => towerPlacer;

    [SerializeField]
    private Text moneyText;

    private void Start()
    {
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
        MoneyBalanceUpdated(this, new MoneyBalanceUpdatedEventArgs(GameControllerS.I.Money, 0));
    }

    private void MoneyBalanceUpdated(object sender, MoneyBalanceUpdatedEventArgs args)
    {
        moneyText.text = "Money: " + args.NewBalance;
    }
}
