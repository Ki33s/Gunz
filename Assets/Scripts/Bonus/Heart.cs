using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public AudioSource takeLife;
    private GameObject gun;
    private bool isTaken = false;
    private float gunDeltaY = 1;
    private float gunDeltaX = 0;


    public void FixedUpdate()
    {
        if (isTaken)
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

            if (gun.GetComponent<Gun>().lifes < gun.GetComponent<Gun>().maxLifes)
            {
                gun.GetComponent<Gun>().lifesArr[gun.GetComponent<Gun>().lifes] = gameObject;
                gun.GetComponent<Gun>().lifes++;

                switch (gun.GetComponent<Gun>().lifes)
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
            if (myTrigger.gameObject.tag == "Duplicate")
            {
                gun = myTrigger.GetComponent<Duplicate>().gun;

                if (gun.GetComponent<Gun>().lifes < gun.GetComponent<Gun>().maxLifes)
                {
                    gun.GetComponent<Gun>().lifesArr[gun.GetComponent<Gun>().lifes] = gameObject;
                    gun.GetComponent<Gun>().lifes++;

                    switch (gun.GetComponent<Gun>().lifes)
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
                    Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
                    Destroy(gameObject);
                }

            }
            else
            {
                Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }

        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void Hitted()
    {
        Destroy(gameObject);
    }


    void Take()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
            if (gun.GetComponent<Gun>().lifes < gun.GetComponent<Gun>().maxLifes)
            {
                gun.GetComponent<Gun>().lifesArr[gun.GetComponent<Gun>().lifes] = gameObject;
                gun.GetComponent<Gun>().lifes++;

                switch (gun.GetComponent<Gun>().lifes)
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
                Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    void Taken()
    {
        transform.gameObject.tag = "Untagged";
        takeLife.Play();
        Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
        Destroy(GetComponent<Collider>());
        transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.x / 2, transform.localScale.x / 2);
        isTaken = true;
    }
}
