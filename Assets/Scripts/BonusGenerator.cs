using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public int panGenProb = 8;
    public int panPlusGenProb = 2;
    public int elitePanGenProb = 100;
    public int elitaPanPlusGenProb = 10;
    public int batGenProb = 8;
    public int batPlusGenProb = 2;
    public int eliteBatGenProb = 80;
    public int elitaBatPlusGenProb = 9;
    public int axeGenProb = 0;
    public int axePlusGenProb = 1;
    public int eliteAxeGenProb = 80;
    public int elitaAxePlusGenProb = 8;
    public int macheteGenProb = 0;
    public int machetePlusGenProb = 1;
    public int eliteMachetenGenProb = 80;
    public int elitaMachetePlusGenProb = 8;
    public int bottlesGenProb = 0;
    public int bottlesPlusGenProb = 1;
    public int eliteBottlesGenProb = 80;
    public int elitaBottlesPlusGenProb = 80;

    public GameObject pan;
    public GameObject bat;
    public GameObject pickaxe;
    public GameObject machete;
    public GameObject bottles;

    public int levelBonusGenProb;
    public int newBonusGenProb;
    public int ammoGenProb = 25;
    public int boostGenProb = 3;
    public int grenadeGenProb = 3;
    public int flashGenProb = 3;
    public int silverCoiGenProb = 10;
    public int goldCoinGenProb = 5;
    public int deathGenProb = 1;
    public int hearthGenProb = 1;
    public int magnetGenProb = 1;


    public GameObject Coin;
    public GameObject Ammo;
    public GameObject Boost;
    public GameObject Grenade;
    public GameObject Flash;
    public GameObject Death;
    public GameObject Heart;
    public GameObject Magnet;

    public Material[] coinMaterials;

    private GameObject gun;
    private bool gameStarted = false;
    private float maxPosY = 5;
    private int deltaY = 15;

    private int enemyDeltaY = 10;
    private int enemySpawnLastUpdateY = 100;
    private int enemySpawnDeltaY = 100;

    public int eventGenProb = 0;
    public int eventDelta = 10;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameStarted)
        {
            FollowGun();
        }
    }

    public void StartGame()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        gameStarted = true;
    }

     void FollowGun()
    {
        if(gun.transform.position.y + deltaY > maxPosY + 0.5f)
        {
            maxPosY += 0.5f;
            GenerateBonuses();

            if (2 * maxPosY % 2 == 0)
            {
                GenerateBoost();
                SpawnEnemy();
                CheckEnemyPlusGenProb();
            }
                
        }
    }

    void SpawnEnemy()
    {
        int randNum = Random.Range(0, 10000);

        if (randNum < panGenProb)
        {
            GameObject enemy = Instantiate(pan, new Vector3(Random.Range(0, 7) - 3, maxPosY - enemyDeltaY - deltaY, -3), Quaternion.identity);
            panGenProb -= panPlusGenProb;

            if (Random.Range(0, 1000) < elitePanGenProb)
            {
                enemy.GetComponent<Enemy>().Elite();
                elitePanGenProb -= elitaPanPlusGenProb;
            } 
        }
        else if (randNum < panGenProb + batGenProb)
        {
            GameObject enemy = Instantiate(bat, new Vector3(Random.Range(0, 5) - 2, maxPosY - enemyDeltaY - deltaY, -3), Quaternion.identity);
            batGenProb -= batPlusGenProb;

            if (Random.Range(0, 1000) < eliteBatGenProb)
            {
                enemy.GetComponent<Enemy>().Elite();
                eliteBatGenProb -= elitaBatPlusGenProb;
            }
        }
        else if (randNum < panGenProb + batGenProb + axeGenProb)
        {
            GameObject enemy = Instantiate(pickaxe, new Vector3(Random.Range(0, 5) - 2, maxPosY - enemyDeltaY - deltaY, -3), Quaternion.identity);
            axeGenProb -= axePlusGenProb;

            if (Random.Range(0, 1000) < eliteAxeGenProb)
            {
                enemy.GetComponent<Enemy>().Elite();
                eliteAxeGenProb -= elitaAxePlusGenProb;
            }
        }
        else if (randNum < panGenProb + batGenProb + axeGenProb + macheteGenProb)
        {
            GameObject enemy = Instantiate(machete, new Vector3(Random.Range(0, 5) - 2, maxPosY - enemyDeltaY - deltaY, -3), Quaternion.identity);
            macheteGenProb -= machetePlusGenProb;

            if (Random.Range(0, 1000) < eliteAxeGenProb)
            {
                enemy.GetComponent<Enemy>().Elite();
                eliteMachetenGenProb -= elitaMachetePlusGenProb;
            }
        }
        else if (randNum < panGenProb + batGenProb + axeGenProb + macheteGenProb + bottlesGenProb)
        {
            GameObject enemy = Instantiate(bottles, new Vector3(Random.Range(0, 7) - 3, maxPosY - enemyDeltaY - deltaY, -3), Quaternion.identity);
            bottlesGenProb -= bottlesPlusGenProb;

            if (Random.Range(0, 1000) < eliteBottlesGenProb)
            {
                enemy.GetComponent<Bottles>().Elite();
                eliteBottlesGenProb -= elitaBottlesPlusGenProb;
            }
        }



    }

    void CheckEnemyPlusGenProb()
    {
        if (maxPosY > enemySpawnLastUpdateY)
        {
            enemySpawnLastUpdateY += enemySpawnDeltaY;

            panGenProb += panPlusGenProb;
            batGenProb += batPlusGenProb;
            axeGenProb += axePlusGenProb;
            macheteGenProb += machetePlusGenProb;
            bottlesGenProb += bottlesPlusGenProb;

            elitePanGenProb += elitaPanPlusGenProb;
            eliteBatGenProb += elitaBatPlusGenProb;
            eliteAxeGenProb += elitaAxePlusGenProb;
            eliteMachetenGenProb += elitaMachetePlusGenProb;
            eliteBottlesGenProb += elitaBottlesPlusGenProb;

            eventGenProb += eventDelta;
        }
    }


    void GenerateBoost()
    {
        if(Random.Range(0,100) < boostGenProb)
            Instantiate(Boost, new Vector3(Random.Range(0,7) - 3, maxPosY, -3), Quaternion.identity);
    }

    void GenerateBonuses()
    {
        if(Random.Range(0, 100) < levelBonusGenProb)
        {
            float coinX = Random.Range(0, 12) / 2 - 2.75f;
            GenerateBonus(coinX, maxPosY);

            if (Random.Range(0, 100) < newBonusGenProb)
            {
                bool right = false;
                if (Random.Range(0, 2) == 0)
                    right = true;

                GenerateNewBonus(coinX, right);
            }
                
        }
    }

    void GenerateNewBonus(float coinX, bool right)
    {
        int newDirection = Random.Range(0, 2);

        if(newDirection == 0)
        {
            maxPosY += 0.5f;
            GenerateBonus(coinX, maxPosY);

            if (Random.Range(0, 100) < newBonusGenProb)
            {
                right = false;
                if (Random.Range(0, 2) == 0)
                    right = true;

                GenerateNewBonus(coinX, right);
            }
        }
        else
        {
            if(right)
            {
                if(coinX < 2.5f)
                {
                    coinX += 0.5f;
                    GenerateBonus(coinX, maxPosY);

                    if (Random.Range(0, 100) < newBonusGenProb)
                        GenerateNewBonus(coinX, right);
                }
            }
            else
            {
                if(coinX > -2.5f)
                {
                    coinX -= 0.5f;
                    GenerateBonus(coinX, maxPosY);

                    if (Random.Range(0, 100) < newBonusGenProb)
                        GenerateNewBonus(coinX, right);
                }
            }
        }
    }

    void GenerateBonus(float coinX, float coinY)
    {
        int randNum = Random.Range(0, 100);

        if (randNum < ammoGenProb)
        {
            Instantiate(Ammo, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else if (randNum < ammoGenProb + grenadeGenProb)
        {
            Instantiate(Grenade, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb)
        {
            Instantiate(Flash, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb)
        {
            GameObject silverCoin = Instantiate(Coin, new Vector3(coinX, coinY, -3), Quaternion.identity);
            silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[0];
            silverCoin.GetComponent<Coin>().coins = 5;
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb)
        {
            GameObject silverCoin = Instantiate(Coin, new Vector3(coinX, coinY, -3), Quaternion.identity);
            silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[1];
            silverCoin.GetComponent<Coin>().coins = 10;
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb + deathGenProb)
        {
            Instantiate(Death, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb + deathGenProb + hearthGenProb)
        {
            Instantiate(Heart, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb + deathGenProb + hearthGenProb + magnetGenProb)
        {
            Instantiate(Magnet, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
        else
        {
            Instantiate(Coin, new Vector3(coinX, coinY, -3), Quaternion.identity);
        }
    }
}
