
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
    public int xSpeed = 3;
    public int eliteXSpeed = 4;

    public int minFollowSpeed = 7;
    public int maxFollowSpeed = 15;
    public float speedDelta = 15;

    public float gunMinDeltaY = 3;
    public float gunMaxDeltaY = 6;
    public int maxDownSpeed = 5;

    public int position;

    private bool selfPlay = false;

    private float followSpeed;
    private float startTimer;
    private float attackTimer = 0;

    private bool attack = false;

    private GameObject gun;

    private bool firstAttack = false;
    public float gunDeltaY;

    private bool elite;

    private float attackDelay = 0;

    private float onAttackTimer = 0.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(selfPlay)
        {
            if (!attack)
            {
                Follow();
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
    }

    public void Elite()
    {
        GetComponent<Outline>().OutlineColor = Color.red;
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

    void CheckNewFollow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY)
        {
            attack = false;
            onAttackTimer = 0.5f;
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

            if (!firstAttack)
                firstAttack = true;

            attackTimer = 0;
            attack = true;
            rig.velocity = new Vector3(rig.velocity.x, 0, 0);
            if (!elite)
            {
                rig.AddForce(new Vector3(0, 1, 0) * Random.Range(minAttackForce, minAttackForce + deltaAttackForce), ForceMode.Force);
            }
            else
            {
                rig.AddForce(new Vector3(0, 1, 0) * Random.Range(eliteMinAttackForce, eliteMinAttackForce + eliteDeltaAttackForce), ForceMode.Force);
            }

            RandomizeDeltaY();
        }
    }

    void Follow()
    {
        if (transform.position.y < gun.transform.position.y - gunDeltaY && followSpeed < maxFollowSpeed)
            followSpeed += speedDelta * Time.deltaTime;
        else if (followSpeed > -1)
            followSpeed -= speedDelta * Time.deltaTime * 2;

        float deltaXSpeed = 0;

        if (!firstAttack)
        {

            if (elite)
                deltaXSpeed = eliteXSpeed * Time.deltaTime;

            if (position == 2)
            {
                if (transform.position.x > 2)
                {
                    position = 0;
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
            else if(position == 0)
            {
                if (transform.position.x < -3)
                {
                    position = 2;
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

    void CheckSpeed()
    {
        if (rig.velocity.y < 0)
        {
            if (rig.velocity.y < -maxDownSpeed)
            {
                rig.velocity = new Vector3(rig.velocity.x, -maxDownSpeed, 0);
            }
        }
    }

    public void Attack()
    {
        transform.parent = null;
        rig = gameObject.AddComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotation;
        rig.drag = 1;
        transform.GetComponent<Collider>().enabled = true;

        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        maxFollowSpeed = gun.GetComponent<Gun>().maxSpeed - 2;

        followSpeed = Random.Range(minFollowSpeed, maxFollowSpeed);

        minAttackForce = (int)(gun.GetComponent<Gun>().force * gunAttackForceCoef);

        if (elite)
            eliteAttackMinTIme = (int)(gun.GetComponent<Gun>().force * eliteGunAttackForceCoef);

        if (!elite)
        {
            startTimer = Random.value * attackRandom + attackMinTime;
        }
        else
        {
            startTimer = Random.value * eliteAttackRandom + eliteAttackMinTIme;
        }

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

            if (gun.GetComponent<Gun>().isEvent)
            {
                //CheckEvent
            }
            else if (gun.GetComponent<Gun>().allItems[11] && Random.Range(0, 100) < gun.GetComponent<Gun>().thornsHelpProb) //CheckThorns
            {
                Shooted();
                gun.GetComponent<Gun>().Rocks();
            }
            else if (gun.GetComponent<Gun>().lifeTaken)
            {
                Shooted();
            }
            else if (gun.GetComponent<Gun>().shopExit)
            {
                //CheckShopExit
            }
            else if (gun.GetComponent<Gun>().skulls > 0)
            {
                Shooted();

                gun.GetComponent<Gun>().skulls--;
                gun.GetComponent<Gun>().skullArr[gun.GetComponent<Gun>().skulls].GetComponent<Death>().Hitted();
            }
            else if (attackDelay <= 0)
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
                    myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * nowForce, ForceMode.Force);
            }



        }
    }

    public void Shooted()
    {
        Instantiate(dieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
