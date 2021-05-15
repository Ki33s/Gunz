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

    public GameObject bottlesRotate;

    private float startTimer;
    private float attackTimer = 0;

    private bool followGun = false;
    private bool dead = false;

    private GameObject gun;
    private Rigidbody rig;

    public float gunDeltaY;
    private bool elite = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();

        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        if (!elite)
            startTimer = Random.value * attackRandom + attackMinTime;
        else
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;

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
        if (myTrigger.gameObject.tag == "Gun" && gun.GetComponent<Gun>().deathTaken)
        {
            Shooted();
        }
    }
}
