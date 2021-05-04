using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingGenerator : MonoBehaviour
{

    public float damping = 4f;
    public float dieGunY = 5f;
    public float CamDeltaY = 5f;
    public int floorsToGenerate = 10;
    public int levelDeltaY = 15;
    private int lastLevelY = 0;

    public int floorBonusGenProb = 20;
    public int newCoinGenProb = 50;

    public int firstBonusDeltaY = 10;
    public int ammoGenProb = 10;
    public int boostGenProb = 10;
    public int grenadeGenProb = 5;
    public int flashGenProb = 5;
    public int silverCoiGenProb = 10;
    public int goldCoinGenProb = 5;

    public GameObject Coin;
    public GameObject Ammo;
    public GameObject Boost;
    public GameObject Grenade;
    public GameObject Flash;

    public Material[] coinMaterials;

    public GameObject[] levelDoors;
    public GameObject[] levelCornerDoors;
    public GameObject[] levels;
    public GameObject[] levelCorners;
    public GameObject[] cars;
    public GameObject[] trees;
    public GameObject[] characters;
    public GameObject[] trash;
    public Material[] materials;
    public GameObject floor;

    public GameObject aircoin;
    public GameObject satelite;
    public GameObject balcony;
    public GameObject[] washline;

    public GameObject bench;

    public int aircoinGenProb = 10;
    public int washlineGenProb = 10;
    public int balconyGenProb = 10;
    public int carGenProb = 50;
    public int benchGenProb = 50;
    public int charGenProb = 50;
    public int treesGenProb = 50;
    public int trashGenProb = 50;

    private int lastLevel = 0;
    private int deltaZ = 0;
    private int nowLevel = 0;

    private int curLevel;
    private int curMaterial;
    private int changeLevel;

    private GameObject[] guns;
    private GameObject gun;

    private GameObject mainLight;

    private float CamLastY;
    private bool gameStarted = false;

    bool[,] coinsArray = new bool[14, 18];
    private float bonusLevel = -0.25f;
    private int bigBonusLevel = 0;

    public GameObject pan;
    public GameObject bat;

    public int firstEnemyFloor = 10;
    public int enemyDeltaY = 10;
    public int panGenProb = 8;
    public int panPlusGenProb = 2;
    public int batGenProb = 8;
    public int batPlusGenProb = 2;

    private int lastEnemyFloor = 0;

    private GameObject leftWall;
    private GameObject rightWall;

    private bool firstBonusGenerated = false;

    private int enemySpawnLastUpdateY = 50;
    private int enemySpawnDeltaY = 50;

    void Start()
    {
       // mainLight = GameObject.Find("Directional Light");
        leftWall = GameObject.Find("WallLeft");
        rightWall = GameObject.Find("WallRight");

        GenerateFloor();
        GenerateDoor();
        GenerateLevels(3*floorsToGenerate);
        LiftWalls();
        GenerateBigBonuses(9 * floorsToGenerate);
        GenerateBonuses(18 * floorsToGenerate);
        lastLevelY = lastLevel;
        firstBonusGenerated = true;
    }

    void FixedUpdate()
    {
        if (gameStarted)
        {
            FollowX();
            CheckNewLevels();
            EnemySpawn();
        }

    }

    public void StartGame()
    {

        guns = GameObject.FindGameObjectsWithTag("Gun");

        foreach (GameObject tempgun in guns)
        {
            gun = tempgun;
        }

        gameStarted = true;
    }

    void LiftWalls()
    {
        leftWall.transform.position = new Vector3(leftWall.transform.position.x, lastLevelY, leftWall.transform.position.z);
        rightWall.transform.position = new Vector3(rightWall.transform.position.x, lastLevelY, rightWall.transform.position.z);
    }

    void CheckNewLevels()
    {
        if (gun.transform.position.y + levelDeltaY > lastLevelY)
        {
            LiftWalls();
            DestroyLowObjects();
            GenerateLevels(floorsToGenerate);
            NullArray();
            GenerateBigBonuses(3 * floorsToGenerate);
            GenerateBonuses(6 * floorsToGenerate);
            lastLevelY = lastLevel;
            //mainLight.transform.position = new Vector3(transform.position.x, lastLevelY, transform.position.z);
        }

    }

    void EnemySpawn()
    {
        if ((int)gun.transform.position.y > lastEnemyFloor + enemyDeltaY)
        {
            SpawnEnemy(lastEnemyFloor);
            lastEnemyFloor = (int)gun.transform.position.y;
        }
    }

    void SpawnEnemy(int floor)
    {
        int randNum = Random.Range(0, 100);

        if (randNum < panGenProb)
        {
            Instantiate(pan, new Vector3(Random.Range(0, 7) - 3, floor, -3), Quaternion.identity);
        }
        else if (randNum < panGenProb + batGenProb)
        {
            Instantiate(bat, new Vector3(Random.Range(0, 5) - 2, floor, -3), Quaternion.identity);
        }
    }

    void GenerateBigBonuses(int floors)
    {
        for (int i = 0; i < floors; i++)
        {
            if(!firstBonusGenerated)
            {
                if(i > firstBonusDeltaY)
                    GenerateBigBonus(i);
                else
                    bigBonusLevel++;
            }
            else
                GenerateBigBonus(i);
        }
    }

    void GenerateBigBonus(int bigArrayY)
    {
        if (Random.Range(0, 100) < boostGenProb)
        {
            int bigArrayX = Random.Range(0, 7);
            int boostX = bigArrayX - 3;

            Instantiate(Boost, new Vector3(boostX, bigBonusLevel, -3), Quaternion.identity);

            coinsArray[bigArrayX * 2, bigArrayY * 2] = true;
            coinsArray[bigArrayX * 2 + 1, bigArrayY * 2] = true;
            coinsArray[bigArrayX * 2, bigArrayY * 2 + 1] = true;
            coinsArray[bigArrayX * 2 + 1, bigArrayY * 2 + 1] = true;
        }

        bigBonusLevel++;
    }

    void GenerateBonuses(int floors)
    {
        

        for (int i = 0; i < floors; i++)
        {
            if (!firstBonusGenerated)
            {
                if (i > firstBonusDeltaY * 2)
                    GenerateBonus(i);
                else
                    bonusLevel += 0.5f;
            }
            else
                GenerateBonus(i);
        }
    }

    void GenerateBonus(int arrayY)
    {
        if (Random.Range(0, 100) < floorBonusGenProb)
        {
            int arrayX = Random.Range(0, 14);
            float coinX = arrayX / 2 - 3.25f;

            if (!coinsArray[arrayX, arrayY])
            {
                int randNum = Random.Range(0, 100);

                if (randNum < ammoGenProb)
                {
                    Instantiate(Ammo, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                }
                else if (randNum < ammoGenProb + grenadeGenProb)
                {
                    Instantiate(Grenade, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                }
                else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb)
                {
                    Instantiate(Flash, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                }
                else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb)
                {
                    GameObject silverCoin = Instantiate(Coin, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                    silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[0];
                    silverCoin.GetComponent<Coin>().coins = 5;                     
                }
                else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb)
                {
                    GameObject silverCoin = Instantiate(Coin, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                    silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[1];
                    silverCoin.GetComponent<Coin>().coins = 10;
                }
                else
                {
                    Instantiate(Coin, new Vector3(coinX, bonusLevel, -3), Quaternion.identity);
                }
                    
                coinsArray[arrayX, arrayY] = true;
                CheckNewCoin(arrayX, arrayY);
            }
        }

        bonusLevel+= 0.5f;
    }

    void CheckNewCoin(int arrayX, int arrayY)
    {
        if (Random.Range(0, 100) < newCoinGenProb)
        {
            int newCoinDirection = Random.Range(0, 3);

            float newCoinX = arrayX / 2 - 3.25f;
            float newCoinY = bonusLevel;

            switch (newCoinDirection)
            {
                case 0:
                    newCoinY = newCoinY + 0.5f;
                    arrayY++;
                    break;

                case 1:
                    newCoinX = newCoinX - 0.5f;
                    arrayX--;
                    break;

                case 2:
                    newCoinX = newCoinX + 0.5f;
                    arrayX++;
                    break;

            }

            if (arrayX >= -0 && arrayX < 14 && arrayY < 6 * floorsToGenerate)
            {
                if (!coinsArray[arrayX, arrayY])
                {
                    int randNum = Random.Range(0, 100);

                    if (randNum < ammoGenProb)
                    {
                        Instantiate(Ammo, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                    }
                    else if (randNum < ammoGenProb + grenadeGenProb)
                    {
                        Instantiate(Grenade, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                    }
                    else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb)
                    {
                        Instantiate(Flash, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                    }
                    else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb)
                    {
                        GameObject silverCoin = Instantiate(Coin, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                        silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[0];
                        silverCoin.GetComponent<Coin>().coins = 5;
                    }
                    else if (randNum < ammoGenProb + grenadeGenProb + flashGenProb + silverCoiGenProb + goldCoinGenProb)
                    {
                        GameObject silverCoin = Instantiate(Coin, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                        silverCoin.transform.GetChild(0).GetComponent<Renderer>().material = coinMaterials[1];
                        silverCoin.GetComponent<Coin>().coins = 10;
                    }
                    else
                    {
                        Instantiate(Coin, new Vector3(newCoinX, newCoinY, -3), Quaternion.identity);
                    }

                    coinsArray[arrayX, arrayY] = true;
                    CheckNewCoin(arrayX, arrayY);
                }
            }

        }
    }



    void NullArray()
    {
        for (int x = 0; x < 14; x++)
        {
            for (int y = 0; y < 18 * floorsToGenerate; y++)
            {
                coinsArray[x, y] = false;
            }
        }
    }

    void DestroyLowObjects()
    {
        foreach (GameObject gObject in FindObjectsOfType(typeof(GameObject)))
        {
            if (gObject.transform.position.y < lastLevelY - 2 * levelDeltaY && gObject.tag != "GameSystem")
                Destroy(gObject);
        }
    }

    void GenerateLevels(int floors)
    {

        for (int i = 0; i < floors; i++)
        {
            GenerateLevel();
        }
    }

    void GenerateFloor()
    {
        GameObject newFloor = Instantiate(floor, new Vector3(0, -0.1f, deltaZ - 3), Quaternion.identity);

        int newMaterial = Random.Range(0, 12);

        for (int i = 0; i < 6; i++)
        {
            newFloor.transform.GetChild(i).GetComponent<Renderer>().material = materials[newMaterial];
        }

        GenerateVehicles();
        GenerateBenches();
        GenerateTrees();
        GenerateTrash();
    }

    void FollowX()
    {
        if (gun.transform.position.y > CamLastY)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, gun.transform.position.y + CamDeltaY, transform.position.z), damping * Time.deltaTime);
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().Distance = (int)CamLastY * 10;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateDistance();

            CheckEnemyPlusGenProb();
        }
        else if (gun.transform.position.y < transform.position.y - dieGunY)
        {
            // gun.GetComponent<Gun>().Die();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }


        CamLastY = gun.transform.position.y;
    }

    void CheckEnemyPlusGenProb()
    {
        if(CamLastY > enemySpawnLastUpdateY)
        {
            enemySpawnLastUpdateY += enemySpawnDeltaY;

            panGenProb += panPlusGenProb;
            batGenProb += batPlusGenProb;
        }
    }

    void GenerateVehicles()
    {
        if(Random.Range(0,100) < carGenProb)
        {
            int carRotationY = 90;
            if (Random.Range(0, 2) == 1)
                carRotationY = -90;

            GameObject newCar = Instantiate(cars[Random.Range(0,8)], new Vector3(4, -0.5f, deltaZ - 8), Quaternion.Euler(0, carRotationY, 0));
            newCar.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < carGenProb)
        {
            int carRotationY = 90;
            if (Random.Range(0, 2) == 1)
                carRotationY = -90;

            GameObject newCar = Instantiate(cars[Random.Range(0, 8)], new Vector3(-4, -0.5f, deltaZ - 8), Quaternion.Euler(0, carRotationY, 0));
            newCar.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }
    }

    void GenerateBenches()
    {
        if (Random.Range(0, 100) < benchGenProb)
        {
            GameObject newBench = Instantiate(bench, new Vector3(-2, -0.5f, deltaZ - 5.5f), Quaternion.Euler(0, 90, 0));
            newBench.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];

           /* if (Random.Range(0, 100) < charGenProb)
            {
                GameObject newChar = Instantiate(characters[Random.Range(0, 9)], new Vector3(-1.65f, -0.44f, deltaZ - 6), Quaternion.Euler(0, 90, 0));
                newChar.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
            }

            if (Random.Range(0, 100) < charGenProb)
            {
                GameObject newChar = Instantiate(characters[Random.Range(0, 9)], new Vector3(-1.65f, -0.44f, deltaZ - 5), Quaternion.Euler(0, 90, 0));
                newChar.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];

            } */
        }

        if (Random.Range(0, 100) < benchGenProb)
        {
            GameObject newBench = Instantiate(bench, new Vector3(2, -0.5f, deltaZ - 5.5f), Quaternion.Euler(0, -90, 0));
            newBench.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];

            /* if (Random.Range(0, 100) < charGenProb)
            {
                GameObject newChar = Instantiate(characters[Random.Range(0,9)], new Vector3(1.65f, -0.44f, deltaZ - 6), Quaternion.Euler(0, -90, 0));
                newChar.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
            }

            if (Random.Range(0, 100) < charGenProb)
            {
                GameObject newChar = Instantiate(characters[Random.Range(0, 9)], new Vector3(1.65f, -0.44f, deltaZ - 5), Quaternion.Euler(0, -90, 0));
                newChar.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
            } */
        }
    }

    void GenerateTrees()
    {
        if (Random.Range(0, 100) < treesGenProb)
        {
            GameObject newTree = Instantiate(trees[Random.Range(0,5)], new Vector3(-3, -0.5f, deltaZ - 5.5f), Quaternion.Euler(0, 90, 0));
            newTree.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < treesGenProb)
        {
            GameObject newTree = Instantiate(trees[Random.Range(0, 5)], new Vector3(3, -0.5f, deltaZ - 5.5f), Quaternion.Euler(0, -90, 0));
            newTree.transform.GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }
    }

    void GenerateTrash()
    {
        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(3f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0,360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(-3f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0, 360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(1.5f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0, 360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(-1.5f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0, 360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(2.25f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0, 360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

        if (Random.Range(0, 100) < trashGenProb)
        {
            GameObject newTrash = Instantiate(trash[Random.Range(0, 9)], new Vector3(-2.25f, 0, deltaZ - 1f), Quaternion.Euler(0, Random.Range(0, 360), 0));
            newTrash.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }
    }

    void GenerateLevel()
    {
        if (nowLevel > changeLevel)
        {
            changeLevel = Random.Range(0, 5) + nowLevel;
            curLevel = Random.Range(0, 3);
            curMaterial = Random.Range(0, 12);
        }

        GenerateFrontLevel();
        GenerateLeftLevel();
        GenerateRightLevel();

        lastLevel += 3;
        nowLevel++;
    }

    void GenerateFrontLevel()
    {
        GameObject newLevel = Instantiate(levels[curLevel], new Vector3(0, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        newLevel = Instantiate(levels[curLevel], new Vector3(-5, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        newLevel = Instantiate(levels[curLevel], new Vector3(5, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        bool isBalcony = false;
        int randNum = Random.Range(0, 100);

        if (randNum < aircoinGenProb)
        {
                int aircoinPos = Random.Range(0, 4);
                float aircoinX = 2.5f;

                switch(aircoinPos)
                {
                    case 0:
                        aircoinX = -2.5f;
                        break;

                    case 1:
                        if (curLevel != 2)
                            aircoinX = 0.85f;
                        else
                            aircoinX = -2.5f;
                        break;

                    case 2:
                        if (curLevel != 2)
                            aircoinX = -0.85f;
                        break;
                }   
                
            if(Random.Range(0,2) == 0)
            {
                GameObject newAircoin = Instantiate(aircoin, new Vector3(aircoinX, lastLevel, deltaZ - 1), Quaternion.identity);
                newAircoin.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
            }
            else
            {
                GameObject newSatelite = Instantiate(satelite, new Vector3(aircoinX, lastLevel, deltaZ - 1), Quaternion.identity);
                newSatelite.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
            }
            
        }
        else if(curLevel == 2 && randNum < aircoinGenProb + balconyGenProb)
        {
            isBalcony = true;
            GameObject newBalcony = Instantiate(balcony, new Vector3(0, lastLevel, deltaZ ), Quaternion.identity);
            newBalcony.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }
       


        if (!isBalcony && Random.Range(0, 100) < washlineGenProb)
        {
            float washX = 1.8f;
            int washRotY = 45;

            if (Random.Range(0, 2) == 1)
            {
                washX = -1.8f;
                washRotY = -45;
            }
                

            GameObject newWashLine = Instantiate(washline[Random.Range(0, 3)], new Vector3(washX, lastLevel - 0.75f, deltaZ - 2.3f), Quaternion.Euler(0, washRotY, 0));
            newWashLine.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
        }

       }

    void GenerateLeftLevel()
    {
        GameObject newLevel = Instantiate(levelCorners[curLevel], new Vector3(-4.3f, lastLevel, deltaZ - 2), Quaternion.Euler(0, -90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        float aircoinZ = 2.85f;
        float aircoinX = 3.3f;
        if (curLevel == 2)
        {
            //aircoinZ = 3.5f;
            aircoinZ = 3f;
            aircoinX = 3.4f;
        }
            

            if (Random.Range(0, 100) < aircoinGenProb)
            {
                if(Random.Range(0,2) == 0)
                {
                    GameObject newAircoin = Instantiate(aircoin, new Vector3(-aircoinX, lastLevel, deltaZ - aircoinZ), Quaternion.Euler(0, -90, 0));
                    newAircoin.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
                }
                else
                {
                    GameObject newSatelite = Instantiate(satelite, new Vector3(-aircoinX, lastLevel, deltaZ - aircoinZ), Quaternion.Euler(0, -90, 0));
                    newSatelite.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
                }
                    
            }
        
    }

    void GenerateRightLevel()
    {
        GameObject newLevel = Instantiate(levelCorners[curLevel], new Vector3(6.3f, lastLevel, deltaZ - 4), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        float aircoinZ = 2.85f;
        float aircoinX = 3.3f;
        if (curLevel == 2)
        {
            //aircoinZ = 3.5f;
            aircoinZ = 3f;
            aircoinX = 3.4f;
        }

            if (Random.Range(0, 100) < aircoinGenProb)
            {
                if(Random.Range(0,2) == 0)
                {
                    GameObject newAircoin = Instantiate(aircoin, new Vector3(aircoinX, lastLevel, deltaZ - aircoinZ), Quaternion.Euler(0, 90, 0));
                    newAircoin.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
                }
                else
                {
                    GameObject newSatelite = Instantiate(satelite, new Vector3(aircoinX, lastLevel, deltaZ - aircoinZ), Quaternion.Euler(0, 90, 0));
                    newSatelite.transform.GetChild(0).GetComponent<Renderer>().material = materials[Random.Range(0, 12)];
                }
                
            }
    }

    void GenerateDoor()
    {
        int door = Random.Range(0, 2);

        GameObject newLevel = Instantiate(levelDoors[door], new Vector3(0, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        newLevel = Instantiate(levelDoors[door], new Vector3(-5, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        newLevel = Instantiate(levelDoors[door], new Vector3(5, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        //newLevel = Instantiate(levels[curLevel], new Vector3(-5, lastLevel, deltaZ), Quaternion.identity);
        //newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        //newLevel = Instantiate(levels[curLevel], new Vector3(5, lastLevel, deltaZ), Quaternion.identity);
        //newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        // newLevel = Instantiate(levels[curLevel], new Vector3(4, lastLevel, deltaZ - 2), Quaternion.Euler(0, 90, 0));
        //newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        newLevel = Instantiate(levelCornerDoors[door], new Vector3(6, lastLevel, deltaZ - 4), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        //newLevel = Instantiate(levels[curLevel], new Vector3(-4, lastLevel, deltaZ - 2), Quaternion.Euler(0, -90, 0));
        //newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        //newLevel = Instantiate(levelCorners[curLevel], new Vector3(-6, lastLevel, deltaZ - 4), Quaternion.Euler(0, -180, 0));
        //newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        newLevel = Instantiate(levelCornerDoors[door], new Vector3(-4, lastLevel, deltaZ - 2), Quaternion.Euler(0, -90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        lastLevel += 3;
        nowLevel++;
    }
}
