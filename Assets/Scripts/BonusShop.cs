using UnityEngine;

public class BonusShop : MonoBehaviour
{
    public GameObject[] bonuses;
    public ParticleSystem[] particles;
    public int[] prices;
    public GameObject noMoney;
    public bool bonusBought = false;

    private int bonusNum;
    private GameObject bonus;
    void Start()
    {
        bonusNum = Random.Range(0, 3);
        bonus = Instantiate(bonuses[bonusNum], transform.position, Quaternion.identity, transform);
    }

    public void BuyBonus()
    {
        int currentCoins = GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins;

        if (currentCoins >= prices[bonusNum])
        {
            bonusBought = true;
            Destroy(gameObject);
            Instantiate(particles[bonusNum], new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);

            foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
            {
                switch (bonusNum)
                {
                    case 0:
                        tempgun.GetComponent<Gun>().ShopBonusAmmo();
                        break;

                    case 1:
                        tempgun.GetComponent<Gun>().ShopBonusSkull();
                        break;

                    case 2:
                        tempgun.GetComponent<Gun>().ShopBonusLife();
                        break;

                }
            }

            
        }
        else
        {
            noMoney.GetComponent<NoMoney>().transp = 255;
        }
    }

}
