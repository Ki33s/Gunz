using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gunRotZ;
    public GameObject trunk;
    public Material redDotMaterial;

    public AudioSource shootSound;
    public AudioSource reloadSound;
    public AudioSource looseLife;
    public AudioSource takeItem;
    public ParticleSystem shotEffect;
    public ParticleSystem takeItemEffect;
    public GameObject boostStrikerBonus;
    public int maxAmmo = 15;
    private int ammo = 15;
    public int force = 500;
    public int newLifeForce = 2000;
    public int maxSpeed = 17;
    public int maxDownSpeed = 5;
    public int rotateSpeed = 360;
    public float recoil_time = 0.5f;
    public int shootDistance = 10;
    public float stunForce = 1.5f;
    public float eliteStunForce = 0.5f;
    private bool recoil = true;
    private float recoil_dt = 0f;
    public bool stun = false;
    public bool eliteStun = false;
    private bool rotRight = false;

    public int maxLifes = 2;
    public int lifes = 0;
    public GameObject[] lifesArr = new GameObject[3];

    public int maxSkulls = 3;
    public int skulls = 0;
    public GameObject[] skullArr = new GameObject[3];
   

    public bool lifeTaken = false;
    private float lifeTimer;

    private Rigidbody rig;

    public bool isEvent = false;

    public int[] allItems = new int[10];

    private int boostStrikerProb;

    private LineRenderer lr;

    private float randItemRecoilTime;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        rig = gameObject.GetComponent<Rigidbody>();
        ChangeRotate();
        randItemRecoilTime = recoil_time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isEvent)
        {
            CheckShoot();
            Rotate();
            CheckRecoil();
            CheckSpeed();
        }
        

        if (lifeTaken)
            CheckDeathTimer();

        if (allItems[2] > 0)
            LaserDot();
    }

    void LaserDot()
    {
        lr.SetPosition(0, trunk.transform.position);
        RaycastHit hit;

        if(Physics.Raycast(trunk.transform.position, transform.forward, out hit))
        {
            if(hit.collider)
            {
                lr.SetPosition(1, hit.point);
            }
            else
            {
                lr.SetPosition(1, transform.forward*10);
            }
        }
    }
    void CheckDeathTimer()
    {
        if (lifeTimer > 0)
            lifeTimer -= Time.deltaTime;
        else
            lifeTaken = false;
    }
    void CheckShoot()
    {
        if ((Input.GetKey("space") || Input.touchCount > 0) && !recoil && ammo > 0)
        {
            if(stun)
                stun = false;

            if (eliteStun)
                eliteStun = false;

            ammo--;
            gunRotZ.GetComponent<GunZRotate>().ChangeRotate();
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().Ammo = ammo;
            GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateAmmo();
            ChangeRotate();
            rig.AddForce(-transform.TransformDirection(Vector3.forward) * force, ForceMode.Force);
            recoil = true;
            shootSound.Play();
            shotEffect.Play();

            Vector3 hitVector = transform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, shootDistance);
            if (isHit)
            {
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<Rigidbody>().AddForce(hitVector * force, ForceMode.Force);
                    hit.transform.SendMessage("Shooted");
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();


                    //ParticleSystem newMetalEffect = Instantiate(metalEffect, hit.point, Quaternion.identity);
                    //newMetalEffect.transform.rotation = transform.rotation;
                }
                else if (hit.transform.tag == "DestroyableEnemy")
                {
                    hit.transform.SendMessage("Shooted");
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                    GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
                }
                else if (hit.transform.tag == "Bonus")
                {
                    hit.transform.SendMessage("Take");
                }
                else if(hit.transform.tag == "Aircoin")
                {
                    hit.transform.GetComponent<Aircoin>().Shooted();
                }
            }

            //BoostStriker
            if(allItems[1]>0)
            {
                if(Random.Range(0,100) < boostStrikerProb)
                {
                  Instantiate(boostStrikerBonus, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void LoseHearth()
    {
        lifeTimer = 1f;
        lifeTaken = true;

        looseLife.Play();
        RefillAmmo();
        rig.AddForce(new Vector3(0, 1, 0) * newLifeForce, ForceMode.Force);
    }

    public void RefillAmmo()
    {
        ammo = maxAmmo;
        reloadSound.Play();
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().Ammo = ammo;
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateAmmo();
    }

    void CheckSpeed()
    {
        float speed = rig.velocity.magnitude;

        if (speed > maxSpeed)
        {
            Vector3 v3Velocity = rig.velocity;
            float vectorPerc = maxSpeed / speed;

            Vector3 newVelocity = new Vector3(v3Velocity.x * vectorPerc, v3Velocity.y * vectorPerc, v3Velocity.z * vectorPerc);
            rig.velocity = newVelocity;
        }

        if (speed > maxDownSpeed && rig.velocity.y < 0)
        {

            Vector3 v3Velocity = rig.velocity;
            float vectorPerc = maxDownSpeed / speed;
            Vector3 newVelocity = new Vector3(v3Velocity.x * vectorPerc, v3Velocity.y * vectorPerc, v3Velocity.z * vectorPerc);
            rig.velocity = newVelocity;
        }

    }

    void ChangeRotate()
    {
        if (transform.eulerAngles.z < 180)
            rotRight = false;
        else
            rotRight = true;
    }


    void Rotate()
    {
        float nowRotateSpeed = rotateSpeed;
        float angleX = transform.eulerAngles.x;
        float k;

        if (angleX < 180)
            k = Mathf.Abs(angleX - 90) / 360 + 0.75f;
        else
            k = 1.25f - Mathf.Abs(angleX - 270)/360;

        nowRotateSpeed *= k;

            if (stun)
            nowRotateSpeed = nowRotateSpeed * stunForce;
        else if (eliteStun)
            nowRotateSpeed = nowRotateSpeed * eliteStunForce;

        if (!rotRight)
                transform.Rotate(Vector3.left * nowRotateSpeed * Time.deltaTime);
            else
                transform.Rotate(Vector3.right * nowRotateSpeed * Time.deltaTime);
    }

    void CheckRecoil()
    {
        if (recoil)
        {
            if (recoil_dt < recoil_time)
            {
                recoil_dt += Time.deltaTime;
            }
            else
            {
                recoil_dt = 0f;
                recoil = false;
            }
        }
    }

    public void TakeItem(int itemID)
    {
        switch(itemID)
        {
            case 0:
                TakeMagazine();
                allItems[0]++;
                break;

            case 1:
                TakeBoostStriker();
                allItems[1]++;
                break;

            case 2:
                TakeRedDot();
                allItems[2]++;
                break;
            case 3:
                TakeBigBullet();
                allItems[3]++;
                break;
            case 4:
                TakeHandle();
                allItems[4]++;
                break;
        }
    }

    private void TakeMagazine()
    {
        maxAmmo += (int)(maxAmmo / 3);
        RefillAmmo();
    }

    private void TakeBoostStriker()
    {
        boostStrikerProb = (int)(40 * randItemRecoilTime);
    }

    private void TakeRedDot()
    {
       lr =  gameObject.AddComponent<LineRenderer>();
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.material = redDotMaterial;
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;
    }

    private void TakeBigBullet()
    {
        force += (int)(force / 3);
        maxSpeed += (int)(maxSpeed / 3);
    }

    private void TakeHandle()
    {
        rotateSpeed += (int)(rotateSpeed / 3);
        recoil_time += (int)(recoil_time / 3);
        gunRotZ.GetComponent<GunZRotate>().rotateSpeed += (int)(gunRotZ.GetComponent<GunZRotate>().rotateSpeed / 3);
    }
}
