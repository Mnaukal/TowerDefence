using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    private Button Button_NextWave;
    [SerializeField]
    private Text Text_WaveNumber;

    void Start()
    {
        GameControllerS.I.WaveController.WaveStarted += WaveController_WaveStarted;
        GameControllerS.I.WaveController.WaveFinished += WaveController_WaveFinished;
    }

    private void WaveController_WaveFinished(object sender, WaveEventArgs args)
    {
        Button_NextWave.interactable = true;
        Text_WaveNumber.text = "";
    }

    private void WaveController_WaveStarted(object sender, WaveEventArgs args)
    {
        Button_NextWave.interactable = false;
        Text_WaveNumber.text = "Wave: " + (args.WaveNumber + 1);
    }

    public void EnableStartButton()
    {
        Button_NextWave.interactable = true;
    }
}
