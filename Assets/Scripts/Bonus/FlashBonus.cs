using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBonus : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public int radius = 5;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            CastFlash();
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void CastFlash()
    {
        float flashY = transform.position.y;

        foreach (GameObject bonus in GameObject.FindGameObjectsWithTag("Bonus"))
        {
           if(Mathf.Abs(flashY - bonus.transform.position.y) < radius)
                bonus.SendMessage("Take");

        }

        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        //foreach (var hitCollider in hitColliders)
        //{
        //  if(hitCollider.tag == "Bonus")
        //   hitCollider.SendMessage("Take");
        //}
    }

    void Take()
    {
        
    }
}
