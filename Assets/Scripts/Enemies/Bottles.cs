using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottles : MonoBehaviour
{
    public int attackRandom = 3;
    public float attackMinTime = 1f;
    public int eliteAttackRandom = 2;
    public float eliteAttackMinTIme = 0;
    public int speed = 20;
    public float gunMinDeltaY = 3;
    public float gunMaxDeltaY = 6;

    public int minFollowSpeed = 7;
    public int maxFollowSpeed = 15;
    public float speedDelta = 15;

    public GameObject bottlesRotate;

    private float startTimer;
    private float attackTimer = 0;

    private bool followGun = false;
    private bool dead = false;

    private GameObject gun;
    private Rigidbody rig;

    private float followSpeed;

    private float gunDeltaY;
    private bool elite = false;

    private bool firstAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();

        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        speed = gun.GetComponent<Gun>().maxSpeed;
        maxFollowSpeed = gun.GetComponent<Gun>().maxSpeed - 2;

        RandomizeDeltaY();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            if (!followGun)
            {
                StartFollow();
                CheckFollow();
            }
            else
            {
                Follow();

                if (!firstAttack)
                    CheckFirstAttack();
                else
                    CheckAttack();
            }
        }
    }

    public void Elite()
    {
        bottlesRotate.GetComponent<BottlesRotate>().Elite();
        elite = true;
    }

    void RandomizeDeltaY()
    {
        gunDeltaY = Random.Range(gunMinDeltaY, gunMaxDeltaY);

        followSpeed = Random.Range(minFollowSpeed, maxFollowSpeed);

        if (!elite)
        {
            startTimer = Random.value * attackRandom + attackMinTime;
        }
        else
        {
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;
        }
    }

    void Follow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY && followSpeed < maxFollowSpeed)
            followSpeed += speedDelta * Time.deltaTime;
        else if (followSpeed > -1)
            followSpeed -= speedDelta * Time.deltaTime * 2;

        rig.velocity = new Vector3(rig.velocity.x, followSpeed, 0);
    }

    void CheckFollow()
    {
        if (transform.position.y > gun.transform.position.y - 7)
        {
            followGun = true;
        }
    }

    void StartFollow()
    {
        rig.velocity = new Vector3(0, speed, 0);
    }

    void CheckFirstAttack()
    {
        if (transform.position.y > gun.transform.position.y - gunDeltaY)
        {
            firstAttack = true;
        }
    }

    void CheckAttack()
    {
        if (attackTimer < startTimer)
        {
            attackTimer += Time.deltaTime;
        }
        else 
        {
            bottlesRotate.GetComponent<BottlesRotate>().gunDeltaY = gunDeltaY;
            bottlesRotate.GetComponent<BottlesRotate>().Attack();
            Destroy(gameObject);
        }
    }

    public void Shooted()
    {
        Destroy(gameObject);
        transform.GetChild(0).transform.SetParent(null);
        bottlesRotate.GetComponent<BottlesRotate>().Shooted();
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (gun.GetComponent<Gun>().allItems[11] && Random.Range(0, 100) < gun.GetComponent<Gun>().thornsHelpProb) //CheckThorns
        {
            Shooted();
            gun.GetComponent<Gun>().Rocks();
        }
        else if (myTrigger.gameObject.tag == "Gun" && gun.GetComponent<Gun>().skulls > 0)
        {
            Shooted();

            gun.GetComponent<Gun>().skulls--;
            gun.GetComponent<Gun>().skullArr[gun.GetComponent<Gun>().skulls].GetComponent<Death>().Hitted();
        }
        else if (myTrigger.gameObject.tag == "Gun" && gun.GetComponent<Gun>().lifeTaken)
        {
            Shooted();
        }
   }
}
