using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public int attackRandom = 3;
    public float attackMinTime = 0.5f;
    public int startForce = 2000;
    public int attackForce = 2000;
    public int hitForce = 500;
    public int gunDeltaY = 3;
    public int speed = 20;
    public int maxDownSpeed = 5;
    public float attackDeltaXVector = 1f;

    public AudioSource dieSound;
    public AudioSource hitSound;

    private float startTimer;
    private float attackTimer = 0;

    private bool attack = false;
    private bool followGun = false;
    private bool dead = false;

    private GameObject[] guns;
    private GameObject gun;
    private Rigidbody rig;

    public int rotateSpeed = 720;
    private bool right = true;

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
            float DeltaXVector = (Random.value * 2 - 1) * attackDeltaXVector;
            rig.AddForce(new Vector3(DeltaXVector, 1, 0) * attackForce, ForceMode.Force);
            attackTimer = 0;
            startTimer = Random.value * attackRandom + attackMinTime;
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
        if (myTrigger.gameObject.tag == "Gun")
        {
            hitSound.Play();
            myTrigger.gameObject.GetComponent<Gun>().stun = true;
            myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * hitForce, ForceMode.Force);
        }
    }

    public void Shooted()
    {
        if (!dead)
        {
            dieSound.Play();
            dead = true;
            Destroy(gameObject, 3);
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
