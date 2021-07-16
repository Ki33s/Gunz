using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gunRotZ;
    public GameObject trunk;
    public Material redDotMaterial;

    public GameObject bulletTrail;

    public AudioSource shootSound;
    public AudioSource reloadSound;
    public AudioSource noAmmo;
    public AudioSource looseLife;
    public ParticleSystem shotEffect;
    public ParticleSystem shellEffect;
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

    private GameObject cameraMain;

    public GameObject TakeSkullGo;
    public GameObject TakeLifeGo;


    public int maxLifes = 2;
    public int lifes = 0;
    public GameObject[] lifesArr = new GameObject[3];

    public int maxSkulls = 3;
    public int skulls = 0;
    public GameObject[] skullArr = new GameObject[3];


    public bool lifeTaken = false;
    private float lifeTimer;

    public bool shopExit = false;
    private float shopExitTImer;

    private Rigidbody rig;

    public bool isEvent = false;

    public bool[] allItems = new bool[20];
    public int itemsNum = 0;

   //BoostStrike
    private int boostStrikerProb;

    //RedDotItem
    public ParticleSystem redDotEffect;

    private float randItemRecoilTime;

    //GrenadeItem
    public GameObject grenade;
    private int grenadeCastProb;

    //FlashItem
    public GameObject flash;
    private int flashCastProb;

    //ReserveAmmoItem
    private int reserveAmmoGenProb;
    public ParticleSystem reserveAmmoEffect;

    //RocksItem
    public int thornsHelpProb = 25;
    public ParticleSystem rocksEffect;

    //PocketItem
    public GameObject pocket;

    //bulletShield
    public GameObject bulletShield;

    //CoinStriker
    public GameObject coin;
    private int coinStrikeProb;

    //Ricochet
    private int ricochetProb;

    //Lighting
    private int lightingProb;
    public GameObject lighting;
    public int lightingRadius = 5;
    public int lightingChain = 5;

    //Blink
    private int blinkProb;
    public ParticleSystem blink;
    public int blinkRange = 5;

    //Duplicate
    private int duplicateProb;
    public int duplicateLife = 5;
    public GameObject duplicate;
    private GameObject curDuplicate;
    private float duplicateCurTimer = 0;
    private bool isDuplicate = false;

    //DoubleBullet
    public int secondBulletDeltaAngle = 45;

    //ShopBonuses
    public GameObject shopAmmo;
    public GameObject shopSkull;
    public GameObject shopLife;
    void Start()
    {
        ammo = maxAmmo;
        rig = gameObject.GetComponent<Rigidbody>();
        ChangeRotate();
        randItemRecoilTime = recoil_time;
        recoil_dt = recoil_time / 2;
        cameraMain = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isEvent)
        {
            CheckShoot();
            Rotate();
            CheckRecoil();
            CheckSpeed();
        }

        if (isDuplicate)
            CheckDuplicate();

        if (lifeTaken)
            CheckDeathTimer();

        if (shopExit)
            CheckShopExitTimer();

    }

    void CheckShopExitTimer()
    {
        if (shopExitTImer > 0)
            shopExitTImer -= Time.deltaTime;
        else
            shopExit = false;
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
        if ((Input.GetKey("space") || Input.touchCount > 0) && !recoil)
        {
            if(ammo > 0)
            {
                if (stun)
                    stun = false;

                if (eliteStun)
                    eliteStun = false;

                ammo--;
                gunRotZ.GetComponent<GunZRotate>().ChangeRotate();
                cameraMain.GetComponent<ScoreManager>().Ammo = ammo;
                cameraMain.GetComponent<ScoreManager>().updateAmmo();
                ChangeRotate();
                rig.AddForce(-transform.TransformDirection(Vector3.forward) * force, ForceMode.Force);
                recoil = true;
                shootSound.Play();
                shotEffect.Play();
                shellEffect.Play();


                GameObject bullet = Instantiate(bulletTrail, transform.position, transform.rotation);

                //GrenadeBullet
                if (allItems[25])
                    bullet.GetComponent<BulletTrail>().explosiveBullet = true;

                //BigBullet
                if (allItems[3])
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * 2, bullet.transform.localScale.x * 2, bullet.transform.localScale.x * 2);

                //PiersingBullet
                if (allItems[12])
                    bullet.GetComponent<BulletTrail>().piersing = true;

                //RichochetBullet

                if (allItems[19])
                {
                    bullet.GetComponent<BulletTrail>().richochet = true;
                    bullet.GetComponent<BulletTrail>().richochetProb = ricochetProb;
                }

                if (allItems[23])
                {
                    bullet.GetComponent<BulletTrail>().lifeVampire = true;
                }

                if (allItems[24])
                {
                    bullet.GetComponent<BulletTrail>().skullVampire = true;
                }


                //BoostStriker
                if (allItems[1])
                {
                    if (Random.Range(0, 100) < boostStrikerProb)
                    {
                        Instantiate(boostStrikerBonus, transform.position, Quaternion.identity);
                    }
                }

                //GrenadeItem
                if (allItems[7])
                {
                    if (Random.Range(0, 100) < grenadeCastProb)
                    {
                        Instantiate(grenade, transform.position, Quaternion.identity);
                    }
                }

                //FlashItem
                if (allItems[8])
                {
                    if (Random.Range(0, 100) < flashCastProb)
                    {
                        Instantiate(flash, transform.position, Quaternion.identity);
                    }
                }

                //ReserveAmmoItem
                if (allItems[10])
                {
                    if (Random.Range(0, 100) < reserveAmmoGenProb)
                    {
                        RefillAmmo();
                        reserveAmmoEffect.Play();
                    }
                }

                //CoinStriker
                if (allItems[15])
                {
                    if (Random.Range(0, 100) < coinStrikeProb)
                    {
                        Instantiate(coin, transform.position, Quaternion.identity);
                    }
                }

                //Lighting
                if (allItems[20])
                {
                    if (Random.Range(0, 100) < lightingProb)
                    {
                        CastLighting(transform.position, lightingChain);
                    }
                }

                //Blink
                if (allItems[21])
                {
                    if (Random.Range(0, 100) < blinkProb)
                    {
                        CastBlink();
                    }
                }

                //Duplicate
                if (allItems[22])
                {
                    if (isDuplicate)
                    {
                        curDuplicate.GetComponent<Duplicate>().Shoot();
                    }

                    if (Random.Range(0, 100) < duplicateProb)
                    {
                        if (isDuplicate)
                        {
                            duplicateCurTimer += duplicateLife;
                            curDuplicate.GetComponent<Duplicate>().duplicateEffect.Play();
                        }
                        else
                            CastDuplicate();
                    }
                }

                //DoubleBullet
                if(allItems[26])
                {
                    GameObject bullet2 = Instantiate(bulletTrail, transform.position, transform.rotation);
                    bullet2.transform.eulerAngles = new Vector3(transform.eulerAngles.x + Random.Range(-secondBulletDeltaAngle, secondBulletDeltaAngle), transform.eulerAngles.y, transform.eulerAngles.z);
                }
            }
            else
            {
                noAmmo.Play();
            }
            
        }
    }

    public void ExitShop()
    {
        shopExitTImer = 2f;
        shopExit = true;
    }

    public void LoseHearth()
    {
        lifeTimer = 2f;
        lifeTaken = true;

        looseLife.Play();
        RefillAmmo();
        rig.AddForce(new Vector3(0, 1, 0) * newLifeForce, ForceMode.Force);
    }

    public void RefillAmmo()
    {
        ammo = maxAmmo;
        reloadSound.Play();
        cameraMain.GetComponent<ScoreManager>().Ammo = ammo;
        cameraMain.GetComponent<ScoreManager>().updateAmmo();
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
            k = 1.25f - Mathf.Abs(angleX - 270) / 360;

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
    public void Rocks()
    {
        rocksEffect.Play();
    }

    private void CastLighting(Vector3 lightingStart, int chain)
    {
        float hitDistance = 5;
        GameObject hitObject = null;
        Collider[] hitColliders = Physics.OverlapSphere(lightingStart, lightingRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Bonus" || hitCollider.tag == "Enemy" || hitCollider.tag == "DestroyableEnemy")
            {
                if(!hitCollider.GetComponent<ItemsCasted>().lighted)
                {
                    float magnitude = (lightingStart - hitCollider.transform.position).magnitude;
                    if (magnitude < hitDistance)
                    {
                        hitDistance = magnitude;
                        hitObject = hitCollider.gameObject;
                    }
                }
            }
        }

        if (hitDistance < 5)
        {
            GameObject newLighting = Instantiate(lighting, lightingStart, Quaternion.identity);
            
            newLighting.transform.localScale = new Vector3(transform.localScale.x, hitDistance, transform.localScale.z);

            newLighting.transform.up = -(lightingStart - hitObject.transform.position).normalized;

            hitObject.GetComponent<ItemsCasted>().lighted = true;

            if (hitObject.tag == "Bonus")
                hitObject.SendMessage("Take");
            else
                hitObject.SendMessage("Shooted");

            if(chain > 0)
            {
                chain--;
                CastLighting(hitObject.transform.position, chain);
            }
        }
    }

    void CastBlink()
    {
        blink.Play();
        transform.position = new Vector3(transform.position.x, transform.position.y + blinkRange, transform.position.z);
    }

    void CastDuplicate()
    {
        duplicateCurTimer = duplicateLife;
        isDuplicate = true;
        curDuplicate = Instantiate(duplicate, transform.position, transform.rotation);
        curDuplicate.GetComponent<Duplicate>().gun = gameObject;
        curDuplicate.GetComponent<Duplicate>().mainGunRotZ = gunRotZ;
        curDuplicate.GetComponent<Duplicate>().isAlive = true;
    }

    void CheckDuplicate()
    {
        duplicateCurTimer -= Time.deltaTime;

        if(duplicateCurTimer < 0 || isEvent)
        {
            curDuplicate.GetComponent<Duplicate>().DestroyIt();
            isDuplicate = false;
        }
    }

    //BonusShop
    public void ShopBonusAmmo()
    {
        Instantiate(shopAmmo, transform.position, Quaternion.identity);
    }

    public void ShopBonusSkull()
    {
        Instantiate(shopSkull, transform.position, Quaternion.identity);
    }

    public void ShopBonusLife()
    {
        Instantiate(shopLife, transform.position, Quaternion.identity);
    }

    //UpgradesShop
    public void TakeItem(int itemID)
    {
        switch (itemID)
        {
            case 0:
                TakeMagazine();
                allItems[0] = true;
                break;

            case 1:
                TakeBoostStriker();
                allItems[1] = true;
                break;

            case 2:
                TakeRedDot();
                allItems[2] = true;
                break;
            case 3:
                TakeBigBullet();
                allItems[3] = true;
                break;
            case 4:
                TakeHandle();
                allItems[4] = true;
                break;
            case 5:
                TakeSkullItem();
                allItems[5] = true;
                break;
            case 6:
                TakeLifeItem();
                allItems[6] = true;
                break;
            case 7:
                TakeGrenadeItem();
                allItems[7] = true;
                break;
            case 8:
                TakeFlashItem();
                allItems[8] = true;
                break;
            case 9:
                TakeShieldBullet();
                allItems[9] = true;
                break;
            case 10:
                TakeReserveAmmoItem();
                allItems[10] = true;
                break;
            case 11:
                allItems[11] = true;
                break;
            case 12:
                allItems[12] = true;
                break;
            case 13:
                TakePocket();
                allItems[13] = true;
                break;
            case 14:
                allItems[14] = true;
                break;
            case 15:
                TakeReserveCoinItem();
                allItems[15] = true;
                break;
            case 17:
                TakeBonusLovers();
                allItems[17] = true;
                break;
            case 18:
                TakeEnemyHater();
                allItems[18] = true;
                break;
            case 19:
                TakeRicochet();
                allItems[19] = true;
                break;
            case 20:
                TakeLighting();
                allItems[20] = true;
                break;
            case 21:
                TakeBlink();
                allItems[21] = true;
                break;

            case 22:
                TakeDuplicate();
                allItems[22] = true;
                break;

            case 23:
                allItems[23] = true;
                break;

            case 24:
                allItems[24] = true;
                break;

            case 25:
                allItems[25] = true;
                break;

            case 26:
                allItems[26] = true;
                break;
        }

        itemsNum++;
    }

    private void TakeMagazine()
    {
        maxAmmo += (int)(maxAmmo / 4);
        RefillAmmo();
    }

    private void TakeBoostStriker()
    {
        boostStrikerProb = (int)(40 * randItemRecoilTime);
    }

    private void TakeRedDot()
    {
        redDotEffect.Play();
    }

    private void TakeBigBullet()
    {
        force += (int)(force / 4);
        maxSpeed += (int)(maxSpeed / 4);
    }

    private void TakeHandle()
    {
        rotateSpeed += (int)(rotateSpeed / 4);
        recoil_time -= (int)(recoil_time / 4);
        gunRotZ.GetComponent<GunZRotate>().rotateSpeed += (int)(gunRotZ.GetComponent<GunZRotate>().rotateSpeed / 5);
    }

    private void TakeSkullItem()
    {
        maxSkulls++;
        Instantiate(TakeSkullGo, transform.position, Quaternion.identity);
    }

    private void TakeLifeItem()
    {
        maxLifes++;
        Instantiate(TakeLifeGo, transform.position, Quaternion.identity);
    }

    private void TakeGrenadeItem()
    {
        grenadeCastProb = (int)(10 * randItemRecoilTime);
    }

    private void TakeFlashItem()
    {
        flashCastProb = (int)(10 * randItemRecoilTime);
    }

    private void TakeShieldBullet()
    {
        Instantiate(bulletShield, transform.position, Quaternion.identity);
    }

    private void TakeReserveAmmoItem()
    {
        reserveAmmoGenProb = (int)(5 * randItemRecoilTime);
        RefillAmmo();
    }

    private void TakePocket()
    {
        Instantiate(pocket, transform.position, Quaternion.identity);
    }

    private void TakeReserveCoinItem()
    {
       coinStrikeProb = (int)(25 * randItemRecoilTime);
    }

    private void TakeBonusLovers()
    {
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().levelBonusGenProb += (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().levelBonusGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().newBonusGenProb += (int) GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().newBonusGenProb / 4;
    }

    private void TakeEnemyHater()
    {
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().panGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().panGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().panPlusGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().panPlusGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().batGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().batGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().batPlusGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().batPlusGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().axeGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().axeGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().axePlusGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().axePlusGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().macheteGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().macheteGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().machetePlusGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().machetePlusGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().bottlesGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().bottlesGenProb / 4;
        GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().bottlesPlusGenProb -= (int)GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>().bottlesPlusGenProb / 4;
    }

    private void TakeLighting()
    {
        lightingProb = (int)(10 * randItemRecoilTime);
    }
    private void TakeRicochet()
    {
        ricochetProb = (int)(30 * randItemRecoilTime);
    }

    private void TakeBlink()
    {
        blinkProb = (int)(15 * randItemRecoilTime);
    }

    private void TakeDuplicate()
    {
        duplicateProb = (int)(10 * randItemRecoilTime);
    }
}
