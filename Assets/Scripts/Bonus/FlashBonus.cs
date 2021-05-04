using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBonus : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public int radius = 5;
    private bool dead = false;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            dead = true;
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            CastFlash();
        }
        else if (myTrigger.gameObject.tag == "Enemy")
        {
            dead = true;
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void CastFlash()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
           if(hitCollider.tag == "Bonus")
                hitCollider.SendMessage("Take");
        }
    }

    void Take()
    {
        if(!dead)
        {
            dead = true;
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            CastFlash();
        }
        
    }
}
