using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for showing Money balance and player's Lives
/// </summary>
public class GameInfoUI : MonoBehaviour
{

    [SerializeField]
    private Text moneyText, livesText;

    void Start()
    {
        GameControllerS.I.MoneyBalanceUpdated += MoneyBalanceUpdated;
        MoneyBalanceUpdated(this, new MoneyBalanceUpdatedEventArgs(GameControllerS.I.Money, 0));

        GameControllerS.I.LivesUpdated += LivesUpdated;
        LivesUpdated(this, new LivesUpdatedEventArgs(GameControllerS.I.Lives, 0));
    }

    private void MoneyBalanceUpdated(object sender, MoneyBalanceUpdatedEventArgs args)
    {
        moneyText.text = "Money: " + args.NewBalance;
    }
    private void LivesUpdated(object sender, LivesUpdatedEventArgs args)
    {
        livesText.text = "Lives: " + args.NewLives;
    }
}
