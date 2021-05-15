using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public ParticleSystem takeEffect;

    private GameObject gun;
    private bool isTaken = false;
    private float gunDeltaY = 1f;

    public void FixedUpdate()
    {
        if(isTaken)
        {
            FollowGun();
        }
    }

    void FollowGun()
    {
        transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y + gunDeltaY, gun.transform.position.z);

        if (transform.localScale.x - Time.deltaTime / 20 > 0)
        {
            float newScale = transform.localScale.x - Time.deltaTime/20;
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
            Destroy(gameObject);
    }


    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            myTrigger.gameObject.GetComponent<Gun>().TakeDeath();
            gun = myTrigger.gameObject;
            Taken();
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Take()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            tempgun.GetComponent<Gun>().TakeDeath();
            gun = tempgun;
            Taken();
        }
    }

    void Taken()
    {
        Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        Destroy(GetComponent<Collider>());
        isTaken = true;
    }
}
