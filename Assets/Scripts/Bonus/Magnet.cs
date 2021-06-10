using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public int radius = 5;
    public int bonusSpeed = 5;

    public ParticleSystem takeEffect;
    public AudioSource takeMagnet;
    private GameObject gun;
    private bool isTaken = false;
    private float gunDeltaY = -0.5f;

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (isTaken)
        {
            FollowGun();
            Magnito();
        }
    }

    void Magnito()
    {
        float gunY = gun.transform.position.y;
        float gunX = gun.transform.position.x;

        foreach (GameObject bonus in GameObject.FindGameObjectsWithTag("Bonus"))
        {
            if (Mathf.Abs(gunY - bonus.transform.position.y) < radius)
            {
                float bonusDeltaX = bonusSpeed * Time.deltaTime;
                float bonusDeltaY = bonusDeltaX * 2;

                if (gun.transform.position.x < bonus.transform.position.x)
                    bonusDeltaX *= -1;

                if (gun.transform.position.y < bonus.transform.position.y)
                    bonusDeltaY *= -0.5f;

                bonus.transform.position = new Vector3(bonus.transform.position.x + bonusDeltaX, bonus.transform.position.y + bonusDeltaY, -3);
            }
        }
    }

    void FollowGun()
    {
        transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y + gunDeltaY, gun.transform.position.z);

        if (transform.localScale.x - Time.deltaTime / 20 > 0)
        {
            float newScale = transform.localScale.x - Time.deltaTime / 20;
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
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
            gun = tempgun;
            Taken();
        }
    }

    void Taken()
    {
        takeMagnet.Play();
        Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
        Destroy(GetComponent<Collider>());
        isTaken = true;
    }
}
