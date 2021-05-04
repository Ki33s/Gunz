using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text coinsUI;
    public int Coins = 0;

    public Text crossUI;
    public int Kills = 0;

    public Text ammoUI;
    public int Ammo = 0;

    public Text distanceUI;
    public int Distance = 0;

    public void updateCoins()
    {
        coinsUI.text = Coins.ToString();
    }

    public void updateKills()
    {
        crossUI.text = Kills.ToString();
    }

    public void updateAmmo()
    {
        ammoUI.text = Ammo.ToString();
    }

    public void updateDistance()
    {
        string textDistance = Distance / 10 + " m";
        distanceUI.text = textDistance;
    }
}
