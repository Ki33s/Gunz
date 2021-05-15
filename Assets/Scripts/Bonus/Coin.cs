using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioSource takeCoin;
    public int coins = 1;

    private bool dead = false;

    void FixedUpdate()
    {
        if (dead)
        {
            Animate();
        }
    }

    void Animate()
    {
        if (transform.localScale.x - Time.deltaTime > 0)
        {
            float newScale = transform.localScale.x - Time.deltaTime;
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (!dead)
        {
            if (myTrigger.gameObject.tag == "Gun")
            {
                takeCoin.Play();
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins+= coins;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateCoins();
                dead = true;
            }
            else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
            {
                dead = true;
            }
        }
    }

    void Take()
    {
        takeCoin.Play();
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().Coins += coins;
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateCoins();
        dead = true;
    }
}
