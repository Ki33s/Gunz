using UnityEngine;

public class Duplicate : MonoBehaviour
{
    public ParticleSystem shotEffect;
    public ParticleSystem shellEffect;
    public ParticleSystem duplicateEffect;
    public ParticleSystem destroyEffect;
    public GameObject gunRotZ;
    public GameObject bulletTrail;

    public float minMaxX = 2.9f;

    private float deltaX;
    private float deltaY;

    public GameObject gun;
    public GameObject mainGunRotZ;

    public bool isAlive = false;

    // Start is called before the first frame update
    void Start()
    {

        int angle = Random.Range(0, 360);
        deltaX = 2*Mathf.Cos(angle);
        deltaY = 2*Mathf.Sin(angle);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive)
        {
            SetPos();
        }
    }

    void SetPos()
    {
        float posX = gun.transform.position.x + deltaX;

        if(deltaX > 0)
        {
            if (posX > minMaxX)
                posX = minMaxX;
        }
        else
        {
            if (posX < -minMaxX)
                posX = -minMaxX;
        }

        transform.position = new Vector3(posX, gun.transform.position.y + deltaY, -3);
        transform.rotation = gun.transform.rotation;
        gunRotZ.transform.rotation = mainGunRotZ.transform.rotation;
    }

    public void Shoot()
    {
        shotEffect.Play();
        shellEffect.Play();
        Instantiate(bulletTrail, transform.position, transform.rotation);
    }

    public void DestroyIt()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
