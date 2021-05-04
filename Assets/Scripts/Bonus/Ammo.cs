using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public ParticleSystem takeEffect;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            myTrigger.gameObject.GetComponent<Gun>().RefillAmmo();
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (myTrigger.gameObject.tag == "Enemy")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Take()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            tempgun.GetComponent<Gun>().RefillAmmo();
        }
        Instantiate(takeEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
