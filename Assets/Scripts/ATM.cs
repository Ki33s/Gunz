using UnityEngine;
using UnityEngine.UI;

public class ATM : MonoBehaviour
{

    public GameObject[] bonuses;
    public int[] prices;
    public Sprite[] itemIcons;

    public GameObject bonusPlace;
    public ParticleSystem buyEffect;
    public ParticleSystem levelUpEffect;

    public GameObject noMoney;

    public bool left;
    public int leftItemNum;
    public GameObject rightAtm;

    private int itemNum;
    public bool itemTaken = false;
    private GameObject item;

    private GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        if(left)
        {
            foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
            {
                gun = tempgun;

                if (gun.GetComponent<Gun>().itemsNum < bonuses.Length)
                {
                    itemNum = Random.Range(0, bonuses.Length);
                    GenerateItem();
                }
                else
                    itemTaken = true;
            }
        }
        
    }

    public void GenerateItem()
    {
       bool isItem = false;

        if(left)
        {
            if (gun.GetComponent<Gun>().allItems[itemNum])
                isItem = true;
        }
        else
        {
            foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
            {
                gun = tempgun;
            }
            itemNum = Random.Range(0, bonuses.Length);

            if (gun.GetComponent<Gun>().allItems[itemNum] || itemNum == leftItemNum)
                isItem = true;
        }

        if(!isItem)
        {
            item = Instantiate(bonuses[itemNum], bonusPlace.transform.position, bonusPlace.transform.rotation, bonusPlace.transform);

            if(left)
            {
                if (gun.GetComponent<Gun>().itemsNum + 1 < bonuses.Length)
                {
                    rightAtm.GetComponent<ATM>().leftItemNum = itemNum;
                    rightAtm.GetComponent<ATM>().GenerateItem();
                }
                else
                {
                    rightAtm.GetComponent<ATM>().itemTaken = true;
                }
                    
            }
        }
            
        else
        {
            itemNum++;
            if (itemNum >= bonuses.Length)
            {
                itemNum = 0;
            } 
                
                GenerateItem();  
        }
    }

    public void ItemBuy()
    {
        if(!itemTaken)
        {
            int currentCoins = GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins;

            if (currentCoins >= prices[itemNum])
            {
                //NotRandomItem
                    itemTaken = true;
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins -= prices[itemNum];
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateCoins();
                    buyEffect.Play();
                    levelUpEffect.Play();
                    Destroy(item);

                //RandomItem

                if(itemNum == 16)
                {
                    gun.GetComponent<Gun>().allItems[16] = true;
                    itemNum = Random.Range(0, bonuses.Length);
                    GenerateRandomItem();
                }

                GameObject NewObj = new GameObject();
                        Image NewImage = NewObj.AddComponent<Image>();
                        NewImage.sprite = itemIcons[itemNum];
                        NewObj.GetComponent<RectTransform>().SetParent(GameObject.Find("Canvas").transform);
                        NewObj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                        NewObj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                        NewObj.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
                        NewObj.transform.position = new Vector3(60, Screen.height - 60 - 100 * gun.GetComponent<Gun>().itemsNum, 0);
                        NewObj.gameObject.tag = "GameSystem";
                        NewObj.SetActive(true);

                gun.GetComponent<Gun>().TakeItem(itemNum);
            }
            else
            {
                noMoney.GetComponent<NoMoney>().transp = 255;
            }
        }
        
    }

    void GenerateRandomItem()
    {
        if(gun.GetComponent<Gun>().allItems[itemNum])
        {
            itemNum++;

            if (itemNum >= bonuses.Length)
                itemNum = 0;

            GenerateRandomItem();
        }
    }
}
