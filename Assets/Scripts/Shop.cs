using UnityEngine;

public class Shop : MonoBehaviour
{
    public int shopGenProb = 10;
    public GameObject shopEnter;
    void Start()
    {
       if(Random.Range(0,100) < shopGenProb)
        {
            Instantiate(shopEnter, new Vector3(0, transform.position.y-1, -3), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
