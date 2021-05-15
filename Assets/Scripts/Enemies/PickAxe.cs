using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : MonoBehaviour
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
    public int speed = 15;
    public int maxDownSpeed = 5;
    public int followGunSpeed = 1;
    public int eliteFollowGunSpeed = 2;

    public AudioSource dieSound;
    public AudioSource hitSound;
    public Color outlineColor;

    private float startTimer;
    private float attackTimer = 0;

    private bool attack = false;
    private bool followGun = false;
    private bool dead = false;

    private GameObject[] guns;
    private GameObject gun;
    private Rigidbody rig;

    public int rotateSpeed = 360;
    private bool right = true;

    private float gunDeltaY;
    private bool elite = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            right = false;

        rig = GetComponent<Rigidbody>();

        guns = GameObject.FindGameObjectsWithTag("Gun");

        foreach (GameObject tempgun in guns)
        {
            gun = tempgun;
        }

        startTimer = Random.value * attackRandom + attackMinTime;

        RandomizeDeltaY();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();

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

        startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;
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
            attack = true;
            if (!elite)
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
        float folowGunDelta;

        if(!elite)
            folowGunDelta = followGunSpeed * Time.deltaTime;
        else
            folowGunDelta = eliteFollowGunSpeed * Time.deltaTime;

        if (gun.transform.position.x < transform.position.x)
            folowGunDelta *= -1;

        transform.position = new Vector3(transform.position.x + folowGunDelta, gun.transform.position.y - gunDeltaY, -3);
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
        }
        else
        {
            dieSound.Play();
        }

    }

    void Rotate()
    {
        if (right)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        }
        else
        {
            transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

}
