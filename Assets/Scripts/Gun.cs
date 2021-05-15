using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gunRotZ;

    public AudioSource shootSound;
    public AudioSource reloadSound;
    public ParticleSystem shotEffect;
    public int maxAmmo = 15;
    private int ammo = 15;
    public int force = 500;
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

    public bool deathTaken = false;
    private float deathTimer;

    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        rig = gameObject.GetComponent<Rigidbody>();
        ChangeRotate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckShoot();
        CheckRecoil();
        Rotate();
        CheckSpeed();

        if (deathTaken)
            CheckDeathTimer();
    }

    void CheckDeathTimer()
    {
        if (deathTimer > 0)
            deathTimer -= Time.deltaTime;
        else
            deathTaken = false;
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
        }
    }

    public void TakeDeath()
    {
        deathTimer = 10f;
        deathTaken = true;
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
}
