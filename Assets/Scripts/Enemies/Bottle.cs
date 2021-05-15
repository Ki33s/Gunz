using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public ParticleSystem dieParticles;
    public AudioSource hitSound;
    private Rigidbody rig;

    public int attackRandom = 3;
    public float attackMinTime = 1f;
    public int eliteAttackRandom = 2;
    public float eliteAttackMinTIme = 0;
    public int attackForce = 1000;
    public int hitForce = 500;
    public int eliteAttackForce = 1200;
    public int eliteHitForce = 800;
    public int xSpeed = 3;
    public int eliteXSpeed = 4;

    public float gunMinDeltaY = 3;
    public float gunMaxDeltaY = 6;
    public int maxDownSpeed = 5;

    public int position;

    private bool selfPlay = false;

    private float startTimer;
    private float attackTimer = 0;

    private bool attack = false;

    private GameObject gun;

    private bool firstAttack = false;
    public float gunDeltaY;

    private bool elite;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(selfPlay)
        {
            if (!attack)
            {
                Follow();
                CheckSpeed();
                CheckAttack();
            }
            else
            {
                CheckNewFollow();
            }
        }
    }

    public void Elite()
    {
        GetComponent<Outline>().OutlineColor = Color.red;
        elite = true;
    }

    void RandomizeDeltaY()
    {
        gunDeltaY = Random.Range(gunMinDeltaY, gunMaxDeltaY);
    }

    void CheckNewFollow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY)
        {
            attack = false;
        }
    }

    void CheckAttack()
    {
        if (attackTimer < startTimer && !attack)
        {
            attackTimer += Time.deltaTime;
            rig.velocity = new Vector3(0, 0, 0);
        }
        else if (!attack)
        {
            if(!firstAttack)
            {
                firstAttack = true;
            }

            attack = true;
            rig.AddForce(new Vector3(0, 1, 0) * attackForce, ForceMode.Force);
            attackTimer = 0;

            if(!elite)
                startTimer = Random.value * attackRandom + attackMinTime;
            else
                startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;

            RandomizeDeltaY();
        }
    }

    void Follow()
    {
        if(!firstAttack)
        {
            float nowXSpeed = 0;
            int newXSpeed = xSpeed;

            if (elite)
                newXSpeed = eliteXSpeed;

            if(position == 0)
            {
                if(transform.position.x < -3)
                {
                    position = 2;
                    nowXSpeed = newXSpeed * Time.deltaTime;
                }
                else
                {
                    nowXSpeed = -newXSpeed * Time.deltaTime;
                }
            }


            if (position == 2)
            {
                if (transform.position.x > 3)
                {
                    position = 0;
                    nowXSpeed = -newXSpeed * Time.deltaTime;
                }
                else
                {
                    nowXSpeed = newXSpeed * Time.deltaTime;
                }
            }

            transform.position = new Vector3(transform.position.x + nowXSpeed, gun.transform.position.y - gunDeltaY, -3);
        }
        else
            transform.position = new Vector3(transform.position.x, gun.transform.position.y - gunDeltaY, -3);
    }

    void CheckSpeed()
    {
        if (rig.velocity.y < 0)
        {
            float speed = rig.velocity.magnitude;

            if (speed > maxDownSpeed)
            {

                Vector3 v3Velocity = rig.velocity;
                float vectorPerc = maxDownSpeed / speed;
                Vector3 newVelocity = new Vector3(v3Velocity.x * vectorPerc, v3Velocity.y * vectorPerc, v3Velocity.z * vectorPerc);
                rig.velocity = newVelocity;
            }
        }


    }

    public void Attack()
    {
        transform.parent = null;
        rig = gameObject.AddComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotation;
        transform.GetComponent<Collider>().enabled = true;

        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        if(!elite)
            startTimer = Random.value * attackRandom + attackMinTime;
        else
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;

        selfPlay = true;

        Quaternion tempRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        transform.GetChild(0).transform.rotation = tempRotation;
        transform.GetChild(0).GetComponent<BottleRotate>().Attack();
    }
    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            hitSound.Play();

            if (gun.GetComponent<Gun>().deathTaken)
            {
                Shooted();
            }
            else
            {
                if (!elite)
                {
                    myTrigger.gameObject.GetComponent<Gun>().stun = true;
                    myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * hitForce, ForceMode.Force);
                }
                else
                {
                    myTrigger.gameObject.GetComponent<Gun>().eliteStun = true;
                    myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * eliteHitForce, ForceMode.Force);
                }
            }
           
            
        }
    }

    public void Shooted()
    {
        Instantiate(dieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
