using UnityEngine;

public class ATM : MonoBehaviour
{

    public GameObject[] bonuses;
    public int[] prices;
    public GameObject bonusPlace;
    public ParticleSystem buyEffect;
    public ParticleSystem levelUpEffect;

    private int itemNum;
    private bool itemTaken = false;
    private GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        itemNum = Random.Range(0, bonuses.Length);
        item = Instantiate(bonuses[itemNum], bonusPlace.transform.position, bonusPlace.transform.rotation, bonusPlace.transform);
    }

    public void ItemBuy()
    {
        if(!itemTaken)
        {
            int currentCoins = GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins;

            if (currentCoins >= prices[itemNum])
            {
                itemTaken = true;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins -= prices[itemNum];
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateCoins();
                buyEffect.Play();
                levelUpEffect.Play();
                Destroy(item);
                foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
                {
                    tempgun.GetComponent<Gun>().TakeItem(itemNum);
                }
            }
            else
            {
                Debug.Log("NoMoney");
            }
        }
        
    }
}
