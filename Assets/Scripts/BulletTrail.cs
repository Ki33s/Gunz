using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{

    public int speed = 10;
    public int force = 1000;
    public int richochetProb = 25;
    public int richochetRadius = 5;
    private Rigidbody rig;
    public GameObject explosiveBulletGO;
    public ParticleSystem explodeEffect;
    public bool piersing = false;
    public bool richochet = false;
    public bool lifeVampire = false;
    public int lifeVampireProb = 100;
    public bool skullVampire = false;
    public int skullVampireProb = 100;
    public bool explosiveBullet = false;
    public int explodeRadius = 1;
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        if (explosiveBullet)
            Instantiate(explosiveBulletGO, transform.position, transform.rotation, transform);

        Destroy(gameObject, 3);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = transform.forward * speed;
    }

    void RichochetCast()
    {
        float hitDistance = 5;
        GameObject hitObject = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, richochetRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Bonus" || hitCollider.tag == "Enemy" || hitCollider.tag == "DestroyableEnemy")
            {
                if (!hitCollider.GetComponent<ItemsCasted>().ricosheted)
                {
                    float magnitude = (transform.position - hitCollider.transform.position).magnitude;
                    if (magnitude < hitDistance)
                    {
                        hitDistance = magnitude;
                        hitObject = hitCollider.gameObject;
                    }
                }
                    
            }
        }

        if(hitDistance < 5)
        {
            hitObject.GetComponent<ItemsCasted>().ricosheted = true;
            transform.LookAt(hitObject.transform.position);
        }
    }

    void Explode()
    {
        Instantiate(explodeEffect, transform.position, transform.rotation);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy" || hitCollider.tag == "DestroyableEnemy")
            {
                hitCollider.SendMessage("Shooted");
                Vector3 heading = hitCollider.transform.position - transform.position;
                if (hitCollider.tag == "Enemy")
                    hitCollider.GetComponent<Rigidbody>().AddForce(heading.normalized * force, ForceMode.Force);

                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
            }
            else if (hitCollider.tag == "Aircoin")
            {
                hitCollider.transform.GetComponent<Aircoin>().Shooted();
            }
            else if (hitCollider.tag == "Satelite")
            {
                hitCollider.transform.GetComponent<Satelite>().Shooted();
            }
            else if (hitCollider.tag == "Bonus")
                hitCollider.SendMessage("Take");
        }
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Enemy")
        {
            myTrigger.transform.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Force);
            myTrigger.transform.SendMessage("Shooted");
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();

            if(lifeVampire)
            {
                if(Random.Range(0,100) < lifeVampireProb)
                {
                    foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
                    {
                        tempgun.GetComponent<Gun>().ShopBonusLife();
                    }
                }
            }

            if(skullVampire)
            {
                if (Random.Range(0, 100) < skullVampireProb)
                {
                    foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
                    {
                        tempgun.GetComponent<Gun>().ShopBonusSkull();
                    }
                }
            }

            if(richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
                else if (!piersing)
                    Destroy(gameObject);
            }
            else if (!piersing)
                Destroy(gameObject);

            if(explosiveBullet)
                Explode();

        }
        else if (myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            myTrigger.transform.SendMessage("Shooted");
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();

            if (richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
                else if (!piersing)
                    Destroy(gameObject);
            }
            else if (!piersing)
                Destroy(gameObject);

            if (explosiveBullet)
                Explode();
        }
        else if (myTrigger.transform.tag == "Bonus")
        {
            myTrigger.transform.SendMessage("Take");

            if (richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
                else if (!piersing)
                    Destroy(gameObject);
            }
            else if (!piersing)
                Destroy(gameObject);

            if (explosiveBullet)
                Explode();
        }
        else if (myTrigger.transform.tag == "Aircoin")
        {
            myTrigger.transform.GetComponent<Aircoin>().Shooted();
            if (richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
            }
            else
                Destroy(gameObject);

            if (explosiveBullet)
                Explode();
        }
        else if(myTrigger.transform.tag == "Satelite")
        {
            if(!myTrigger.transform.GetComponent<Satelite>().dead)
                myTrigger.transform.GetComponent<Satelite>().Shooted();

            if (richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
                else if (!piersing)
                    Destroy(gameObject);
            }
            else if (!piersing)
                Destroy(gameObject);

            if (explosiveBullet)
                Explode();
        }
        else if(myTrigger.transform.tag == "LeftWall" || myTrigger.transform.tag == "MidWall" || myTrigger.transform.tag == "RightWall")
        {
            if (richochet)
            {
                if (Random.Range(0, 100) < richochetProb)
                    RichochetCast();
            }
            else
                Destroy(gameObject);

            if (explosiveBullet)
                Explode();
        }

    }

    }
