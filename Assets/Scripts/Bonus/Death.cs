using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public ParticleSystem hitEffect;
    public AudioSource takeDeath;
    private GameObject gun;
    private bool isTaken = false;
    private float gunDeltaY = -0.7f;
    private float gunDeltaX = 0;

    public void FixedUpdate()
    {
        if(isTaken)
        {
            FollowGun();
        }
    }

    void FollowGun()
    {
        transform.position = new Vector3(gun.transform.position.x + gunDeltaX, gun.transform.position.y + gunDeltaY, gun.transform.position.z);
    }


    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            gun = myTrigger.gameObject;

            if(gun.GetComponent<Gun>().skulls < gun.GetComponent<Gun>().maxSkulls)
            {
                myTrigger.gameObject.GetComponent<Gun>().skullArr[gun.GetComponent<Gun>().skulls] = gameObject;
                gun.GetComponent<Gun>().skulls++;

                switch(gun.GetComponent<Gun>().skulls)
                {
                    case 2:
                        gunDeltaX = -0.5f;
                        break;

                    case 3:
                        gunDeltaX = 0.5f;
                        break;

                    case 4:
                        gunDeltaX = 1f;
                        break;
                }

                Taken();
            }
            else
            {
                Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }
            
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Hitted()
    {
        Instantiate(hitEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

    void Take()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
            if (gun.GetComponent<Gun>().skulls < gun.GetComponent<Gun>().maxSkulls)
            {
                gun.GetComponent<Gun>().skullArr[gun.GetComponent<Gun>().skulls] = gameObject;
                gun.GetComponent<Gun>().skulls++;

                switch (gun.GetComponent<Gun>().skulls)
                {
                    case 2:
                        gunDeltaX = -0.5f;
                        break;

                    case 3:
                        gunDeltaX = 0.5f;
                        break;

                    case 4:
                        gunDeltaX = 1f;
                        break;
                }

                Taken();
            }
            else
            {
                Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void Taken()
    {
        transform.gameObject.tag = "Untagged";
        takeDeath.Play();
        Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        Destroy(GetComponent<Collider>());
        transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.x / 2, transform.localScale.x / 2);
        isTaken = true;
    }
}
