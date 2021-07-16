using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int attackRandom = 2;
    public int attackMinTime = 2;

    public int eliteAttackRandom = 1;
    public int eliteAttackMinTIme = 1;

    public int minAttackForce = 500;
    public int maxAttackForce = 1000;
    public int deltaAttackForce = 500;
    public float gunAttackForceCoef = 1.5f;

    public int eliteMinAttackForce = 1000;
    public int eliteMaxAttackForce = 1500;
    public int eliteDeltaAttackForce = 500;
    public float eliteGunAttackForceCoef = 1.5f;

    public int minHitForce = 500;
    public int maxHitForce = 1000;

    public int eliteMinHitForce = 1000;
    public int eliteMaxHitForce = 1500;

    public float gunMinDeltaY = 2f;
    public float gunMaxDeltaY = 5f;

    public int minFollowSpeed = 7;
    public int maxFollowSpeed = 15;
    public float speedDelta = 15;

    public float attackDeltaXVector = 0f;

    public int followGunSpeed = 0;
    public int eliteFollowGunSpeed = 0;

    public int xSpeed = 0;
    public int eliteXSpeed = 0;

    public int speed = 15;
    public int maxDownSpeed = 3;

    public AudioSource hitSound;
    public AudioSource dieSound;

    public Color outlineColor;

    private float startTimer;
    private bool elite = false;
    private Rigidbody rig;
    private GameObject gun;
    private bool dead = false;
    private float gunDeltaY;
    private bool followGun = false;
    private bool attack = false;
    private bool firstAttack = false;

    private float followSpeed;
    private float attackTimer = 0;

    private float onAttackTimer = 0.5f;

    private float attackDelay = 0;

    private bool xRight = true;

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

        minAttackForce = (int)(gun.GetComponent<Gun>().force * gunAttackForceCoef);

        if(elite)
            eliteAttackMinTIme = (int)(gun.GetComponent<Gun>().force * eliteGunAttackForceCoef);

        RandomizeDeltaY();

        if(xSpeed > 0)
            if (Random.Range(0, 2) == 1)
                xRight = false;
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
            else if (!attack)
            {
                Follow();

                if (!firstAttack)
                    CheckFirstAttack();
                else
                    CheckAttack();
            }
            else if (onAttackTimer > 0)
            {
                onAttackTimer -= Time.deltaTime;
            }
            else
            {
                CheckSpeed();
                CheckNewFollow();
            }

            if (attackDelay > 0)
                attackDelay -= Time.deltaTime;
        }
        else
            CheckDeath();
    }

    void CheckDeath()
    {
        if (transform.position.y < gun.transform.position.y - 8)
            Destroy(gameObject);
    }


    void CheckNewFollow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY)
        {
            attack = false;
            onAttackTimer = 0.5f;
        }
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
        if (attackTimer < startTimer && !attack)
        {
            attackTimer += Time.deltaTime;
        }
        else if (!attack)
        {
            attackTimer = 0;
            attack = true;
            rig.velocity = new Vector3(rig.velocity.x, 0, 0);

            float DeltaXVector = 0;

            if (attackDeltaXVector > 0)
                DeltaXVector = (Random.value * 2 - 1) * attackDeltaXVector;

            if (!elite)
            {
                rig.AddForce(new Vector3(DeltaXVector, 1, 0) * Random.Range(minAttackForce, minAttackForce + deltaAttackForce), ForceMode.Force);
            }
            else
            {
                rig.AddForce(new Vector3(DeltaXVector, 1, 0) * Random.Range(eliteMinAttackForce, eliteMinAttackForce + eliteDeltaAttackForce), ForceMode.Force);
            }
            RandomizeDeltaY();
        }
    }

    void CheckSpeed()
    {
        if (rig.velocity.y < 0)
        { 
            if (rig.velocity.y < -maxDownSpeed)
            {
                rig.velocity = new Vector3(rig.velocity.x, -maxDownSpeed,0);
            }
        }
    }

    void Follow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY && followSpeed < maxFollowSpeed)
          followSpeed += speedDelta * Time.deltaTime;
        else if(followSpeed > -1)
           followSpeed -= speedDelta * Time.deltaTime * 2;

        float deltaXSpeed = 0;

        if(followGunSpeed > 0)
        {
            if (!elite)
                deltaXSpeed = followGunSpeed * Time.deltaTime;
            else
                deltaXSpeed = eliteFollowGunSpeed * Time.deltaTime;

            if (gun.transform.position.x < transform.position.x)
                deltaXSpeed *= -1;
        }
        else if(xSpeed > 0)
        {
            if (elite)
                deltaXSpeed = eliteXSpeed * Time.deltaTime;

            if (xRight)
            {
                if (transform.position.x > 2)
                {
                    xRight = false;
                    rig.velocity = new Vector3(0, rig.velocity.y, 0);
                }
                else
                {
                    if (elite)
                        deltaXSpeed = eliteXSpeed * Time.deltaTime;
                    else
                        deltaXSpeed = xSpeed * Time.deltaTime;
                }

            }
            else
            {
                if (transform.position.x < -2)
                {
                    xRight = true;
                    rig.velocity = new Vector3(0, rig.velocity.y, 0);
                }
                else
                {
                    if (elite)
                        deltaXSpeed = -eliteXSpeed * Time.deltaTime;
                    else
                        deltaXSpeed = -xSpeed * Time.deltaTime; 
                }

            }
        }
       

        rig.velocity = new Vector3(rig.velocity.x + deltaXSpeed, followSpeed, 0);
    }

    void RandomizeDeltaY()
    {
        gunDeltaY = Random.Range(gunMinDeltaY,gunMaxDeltaY);
        followSpeed = Random.Range(minFollowSpeed, maxFollowSpeed);

        if(!elite)
        {
            startTimer = Random.value * attackRandom + attackMinTime;
        }
        else
        {
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;
        }
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

    public void Shooted()
    {
        if (!dead)
        {
            dieSound.Play();

            if (!elite)
            {
                dead = true;
                Destroy(GetComponent<Outline>());
                rig.AddForce(new Vector3(0, 0, Random.Range(-1f, 1f)) * 400, ForceMode.Force);
                rig.freezeRotation = false;
                rig.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 400, ForceMode.Force);
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

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" && !dead)
        {
            hitSound.Play();

            if (gun.GetComponent<Gun>().isEvent)
            {
                //CheckEvent
            }
            else if (gun.GetComponent<Gun>().allItems[11] && Random.Range(0,100) < gun.GetComponent<Gun>().thornsHelpProb) //CheckRocks
            {
                Shooted();
                gun.GetComponent<Gun>().Rocks();
            }
            else if(gun.GetComponent<Gun>().lifeTaken) // Check if just life spent
            {
                Shooted();
            }
            else if (gun.GetComponent<Gun>().shopExit)
            {
                //CheckShopExit
            }
            else if (gun.GetComponent<Gun>().skulls > 0) //Check Skulls
            {
                Shooted();

                gun.GetComponent<Gun>().skulls--;
                gun.GetComponent<Gun>().skullArr[gun.GetComponent<Gun>().skulls].GetComponent<Death>().Hitted();
            }
            else if(attackDelay <= 0)
            {
                int nowForce;

                if (!elite)
                    nowForce = Random.Range(minHitForce, maxHitForce);
                else
                    nowForce = Random.Range(eliteMinHitForce, eliteMaxHitForce);

                //CheckThorns
                if (gun.GetComponent<Gun>().allItems[14])
                    nowForce /= 2;

                    attackDelay = 1;
                    //myTrigger.gameObject.GetComponent<Gun>().stun = true;
                    myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * nowForce, ForceMode.Force);
            }
        }
        else if(myTrigger.gameObject.tag == "Aircoin" || myTrigger.gameObject.tag == "Satelite")
        {
            myTrigger.SendMessage("Shooted");
        }
    }
}
