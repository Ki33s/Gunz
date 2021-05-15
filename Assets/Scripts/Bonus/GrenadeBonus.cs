using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBonus : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public int radius = 5;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            CastFlash();
            Destroy(gameObject);
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void CastFlash()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy" || hitCollider.tag == "DestroyableEnemy")
            {
                hitCollider.SendMessage("Shooted");
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
            }
            else if (hitCollider.tag == "Aircoin")
                hitCollider.transform.GetComponent<Aircoin>().Shooted();
        }
    }

    void Take()
    {
        Instantiate(takeEffect, transform.position, Quaternion.identity);
        CastFlash();
        Destroy(gameObject);
    }
}
