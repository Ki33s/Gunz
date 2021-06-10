using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingGenerator : MonoBehaviour
{

    public float damping = 4f;
    public float dieGunY = 5f;
    public int minCamUp = 1;
    public float CamDeltaY = 5f;
    public int floorsToGenerate = 10;
    public int levelDeltaY = 15;
    private int lastLevelY = 0;

    public GameObject[] levelDoors;
    public GameObject[] levels;
    public Material[] materials;

    private int lastLevel = 0;
    private int deltaZ = 0;
    private int nowLevel = 0;

    private int curLevel;
    private int curMaterial;
    private int changeLevel;

    private GameObject gun;

    private float CamLastY;
    private bool gameStarted = false;

    public GameObject leftWall;
    public GameObject rightWall;

    private int wallDeltaY = 12;

    public bool isEvent = false;
    public Vector3 eventCamPlace;

    public int eventExitForce = 1500;

    public GameObject lastEventEnter;

    void Start()
    {
        Application.targetFrameRate = 60;
        GenerateDoor();
        GenerateLevels(4*floorsToGenerate);
        LiftWalls();
        lastLevelY = lastLevel;
    }

    void FixedUpdate()
    {
        if (gameStarted)
        {
            if(!isEvent)
            {
                FollowX();
                CheckNewLevels();
            }
            else
            {
                CheckEvent();
            }
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

    void CheckEvent()
    {
        if (Mathf.Abs((transform.position - eventCamPlace).magnitude) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, eventCamPlace, Time.deltaTime * damping);
        }

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10))
            {
                if(hit.transform.tag == "ATM")
                {
                    hit.transform.GetComponent<ATM>().ItemBuy();
                }
                else if(hit.transform.tag == "Exit")
                {
                    ExitEvent();
                }
            }
        }
    }

    public void ExitEvent()
    {
        lastEventEnter.GetComponent<EventEnter>().GunExit();
        isEvent = false;
        gun.GetComponent<Gun>().isEvent = false;
        gun.GetComponent<Rigidbody>().useGravity = true;
        gun.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * eventExitForce, ForceMode.Force);


    }

    void LiftWalls()
    {
        leftWall.transform.position = new Vector3(leftWall.transform.position.x, lastLevelY - wallDeltaY, leftWall.transform.position.z);
        rightWall.transform.position = new Vector3(rightWall.transform.position.x, lastLevelY - wallDeltaY, rightWall.transform.position.z);
    }

    void CheckNewLevels()
    {
        if (gun.transform.position.y + levelDeltaY > lastLevelY)
        {
            LiftWalls();
            DestroyLowObjects();
            GenerateLevels(floorsToGenerate);
            lastLevelY = lastLevel;
        }
    }

    void DestroyLowObjects()
    {
        foreach (GameObject gObject in FindObjectsOfType(typeof(GameObject)))
        {
            if (gObject.transform.position.y < lastLevelY - 2 * levelDeltaY && gObject.tag != "GameSystem" && gObject.tag != "Enemy")
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
    void FollowX()
    {
        if (gun.transform.position.y > CamLastY)
        {
            CamLastY = gun.transform.position.y;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().Distance = (int)CamLastY * 10;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateDistance();
        }
        else if (gun.transform.position.y < CamLastY - dieGunY)
        {
            if(gun.GetComponent<Gun>().lifes > 0)
            {
                gun.GetComponent<Gun>().LoseHearth();
                gun.GetComponent<Gun>().lifes--;
                gun.GetComponent<Gun>().lifesArr[gun.GetComponent<Gun>().lifes].GetComponent<Heart>().Hitted();
            }
            else
                SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else
        {
            CamLastY += minCamUp * Time.deltaTime;
        }

        float posZ = transform.position.z;

        if (posZ > -15)
            posZ = -15;

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, CamLastY + CamDeltaY, posZ), damping * Time.deltaTime);

        
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
        newLevel.GetComponent<LevelElements>().left = true;

        newLevel = Instantiate(levels[curLevel], new Vector3(5, lastLevel, deltaZ), Quaternion.identity);
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        newLevel.GetComponent<LevelElements>().right = true;
       }

    void GenerateLeftLevel()
    {
        GameObject newLevel = Instantiate(levels[curLevel], new Vector3(-4f, lastLevel, deltaZ - 2), Quaternion.Euler(0, -90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        newLevel.GetComponent<LevelElements>().right = true;
        newLevel.GetComponent<LevelElements>().corner = true;
    }

    void GenerateRightLevel()
    {
        GameObject newLevel = Instantiate(levels[curLevel], new Vector3(4f, lastLevel, deltaZ - 2), Quaternion.Euler(0, 90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];
        newLevel.GetComponent<LevelElements>().left = true;
        newLevel.GetComponent<LevelElements>().corner = true;
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

        newLevel = Instantiate(levelDoors[door], new Vector3(4f, lastLevel, deltaZ - 2), Quaternion.Euler(0, 90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        newLevel = Instantiate(levelDoors[door], new Vector3(-4f, lastLevel, deltaZ - 2), Quaternion.Euler(0, -90, 0));
        newLevel.transform.GetChild(0).GetComponent<Renderer>().material = materials[curMaterial];

        lastLevel += 3;
        nowLevel++;
    }
}
