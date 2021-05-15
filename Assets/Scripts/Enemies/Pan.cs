using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public int attackRandom = 3;
    public float attackMinTime = 0.5f;
    public int eliteAttackRandom = 2;
    public float eliteAttackMinTIme = 0;
    public int attackForce = 2000;
    public int eliteAttackForce = 2000;
    public int hitForce = 500;
    public int eliteHitForce = 1000;
    public float gunMinDeltaY = 3;
    public float gunMaxDeltaY = 6;
    public int speed = 20;
    public int maxDownSpeed = 5;

    public AudioSource dieSound;
    public AudioSource hitSound;
    public Color outlineColor;

    private float startTimer;
    private float attackTimer = 0;

    private bool attack = false;
    private bool followGun = false;
    private bool dead = false;

    private GameObject gun;
    private Rigidbody rig;

    private float gunDeltaY;

    private bool elite = false;

    void Start()
    {
        rig = GetComponent<Rigidbody>();

        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        if(!elite)
            startTimer = Random.value * attackRandom + attackMinTime;
        else
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;

        RandomizeDeltaY();
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            if (!followGun)
            {
                StartFollow();
                CheckFollow();
            }
            else if (!attack)
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

    private void nonElite()
    {
        GetComponent<Outline>().OutlineColor = outlineColor;
        elite = false;

        startTimer = Random.value * attackRandom + attackMinTime;
    }

    void RandomizeDeltaY()
    {
        gunDeltaY = Random.Range(gunMinDeltaY, gunMaxDeltaY);
    }

    void CheckSpeed()
    {
       if(rig.velocity.y < 0)
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
            attack = true;
            if(!elite)
            {
                rig.AddForce(new Vector3(0, 1, 0) * attackForce, ForceMode.Force);
                attackTimer = 0;
                startTimer = Random.value * attackRandom + attackMinTime;
            }
            else
            {
                rig.AddForce(new Vector3(0, 1, 0) * eliteAttackForce, ForceMode.Force);
                attackTimer = 0;
                startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;
            }
            
            RandomizeDeltaY();
        }
    }

    void Follow()
    {
        transform.position = new Vector3(transform.position.x, gun.transform.position.y - gunDeltaY, -3);
    }

    void StartFollow()
    {
        rig.velocity = new Vector3(0, speed, 0);
    }

    void CheckFollow()
    {
        if (transform.position.y > gun.transform.position.y - gunDeltaY)
        {
            followGun = true;
        }
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" && !dead)
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
        if (!dead)
        {
            dieSound.Play();

            if (!elite)
            {
                dead = true;
            }
            else
            {
                nonElite();
            }
            
            
            //Destroy(gameObject, 3);
        }
        else
        {
            dieSound.Play();
        }

    }
}
