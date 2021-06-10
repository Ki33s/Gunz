using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElements : MonoBehaviour
{
    public int flowerGenProb = 30;
    public int aircoinsGenProb = 30;
    public int washLineGenProb = 30;
    public int balconyGenProb = 30;

    public GameObject leftFlowerSpawn;
    public GameObject midFlowerSpawn;
    public GameObject rightFlowerSpawn;

    public GameObject[] aircoinSpawnPoints;
    public GameObject[] washLineSpawnPoints;

    public GameObject[] flowers;
    public GameObject[] aircoins;
    public GameObject[] washlines;
    public GameObject balcony;
    public GameObject shop;
    public Material[] materials;

    public bool type3 = false;
    public bool corner = false;

    public bool left = false;
    public bool right = false;

    private bool isBalcony = false;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFlowers();

        if (!left && !right && !corner)
        {
            if (Random.Range(0, 100) < aircoinsGenProb)
            {
                int aircoinPos = Random.Range(0, 4);
                GameObject aircoin = Instantiate(aircoins[Random.Range(0, aircoins.Length)], aircoinSpawnPoints[aircoinPos].transform.position, aircoinSpawnPoints[aircoinPos].transform.rotation, aircoinSpawnPoints[aircoinPos].transform);
                aircoin.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            }
            else if(type3 && Random.Range(0, 100) < aircoinsGenProb + balconyGenProb)
            {
                isBalcony = true;

                if (Random.Range(0, 100) < GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().eventGenProb)
                {
                    GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().eventGenProb = 0;
                    Instantiate(shop, transform.position, transform.rotation, transform);
                }
                else
                {
                    GameObject newBalcony = Instantiate(balcony, transform.position, transform.rotation, transform);
                    newBalcony.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
                }

                
            }

            if(!isBalcony && Random.Range(0,100) < washLineGenProb)
            {
                int washLinePos = Random.Range(0, 2);
                GameObject washline = Instantiate(washlines[Random.Range(0, washlines.Length)], washLineSpawnPoints[washLinePos].transform.position, washLineSpawnPoints[washLinePos].transform.rotation, washLineSpawnPoints[washLinePos].transform);
                washline.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            }
        }
        else if (corner && Random.Range(0, 100) < aircoinsGenProb)
        {
            int aircoinPos = 1;

             if (left)
                aircoinPos = 2;

            GameObject aircoin = Instantiate(aircoins[Random.Range(0, aircoins.Length)], aircoinSpawnPoints[aircoinPos].transform.position, aircoinSpawnPoints[aircoinPos].transform.rotation, aircoinSpawnPoints[aircoinPos].transform);
            aircoin.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    void GenerateFlowers()
    {
        if (!left && ! corner)
        {
            if (Random.Range(0, 100) < flowerGenProb)
            {
                GameObject leftFlower = Instantiate(flowers[Random.Range(0, flowers.Length)], leftFlowerSpawn.transform.position, leftFlowerSpawn.transform.rotation, leftFlowerSpawn.transform);
                leftFlower.transform.GetChild(0).transform.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            }
        }

        if (((!left && !right) || corner) && !type3)
        {
            if (Random.Range(0, 100) < flowerGenProb)
            {
                GameObject midFlower = Instantiate(flowers[Random.Range(0, flowers.Length)], midFlowerSpawn.transform.position, midFlowerSpawn.transform.rotation, midFlowerSpawn.transform);
                midFlower.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            }
        }

        if (!right && !corner)
        {
            if (Random.Range(0, 100) < flowerGenProb)
            {
                GameObject rightFlower = Instantiate(flowers[Random.Range(0, flowers.Length)], rightFlowerSpawn.transform.position, rightFlowerSpawn.transform.rotation, rightFlowerSpawn.transform);
                rightFlower.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
            }
        }
    }
}

