using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopEnter;
    public GameObject atm;
    public GameObject pallet;
    public GameObject gunPlace;
    void Start()
    {
        if(Random.Range(0,100) < GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().eventGenProb)
        {
            GenShop();
        }
    }

    public void GenShop()
    {
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().eventGenProb = 0;
        Instantiate(atm, new Vector3(-1.95f, transform.position.y + 1.1f, -1), Quaternion.identity);
        Instantiate(atm, new Vector3(1.95f, transform.position.y + 1.1f, -1), Quaternion.identity);
        Instantiate(pallet, new Vector3(0, transform.position.y - 0.15f, -0.45f), Quaternion.identity);
        GameObject eventEnter = Instantiate(shopEnter, new Vector3(0, transform.position.y - 1, -3), Quaternion.identity);
        eventEnter.GetComponent<EventEnter>().gunPlace = gunPlace;
    }
}
