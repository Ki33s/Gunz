using UnityEngine;

public class Ammo : MonoBehaviour
{
    public ParticleSystem takeEffect;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            myTrigger.gameObject.GetComponent<Gun>().RefillAmmo();
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
        else if (myTrigger.gameObject.tag == "Duplicate")
        {
            myTrigger.gameObject.GetComponent<Duplicate>().gun.GetComponent<Gun>().RefillAmmo();
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Take()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            tempgun.GetComponent<Gun>().RefillAmmo();
        }
        Instantiate(takeEffect, new Vector3(transform.position.x, transform.position.y-0.3f, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
