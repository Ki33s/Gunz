using UnityEngine;

public class GrenadeBonus : MonoBehaviour
{
    public ParticleSystem takeEffect;
    public int radius = 5;
    public int hitForce = 500;
    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" || myTrigger.gameObject.tag == "Duplicate")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            CastFlash();
            Destroy(gameObject);
        }
        else if (myTrigger.gameObject.tag == "Enemy" || myTrigger.gameObject.tag == "DestroyableEnemy")
        {
            Instantiate(takeEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void CastFlash()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy" || hitCollider.tag == "DestroyableEnemy")
            {
                hitCollider.SendMessage("Shooted");
                Vector3 heading = hitCollider.transform.position - transform.position;
                if(hitCollider.tag == "Enemy")
                    hitCollider.GetComponent<Rigidbody>().AddForce(heading.normalized * hitForce, ForceMode.Force);

                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
            }
            else if (hitCollider.tag == "Aircoin")
                hitCollider.transform.GetComponent<Aircoin>().Shooted();
            else if (hitCollider.tag == "Satelite")
                hitCollider.transform.GetComponent<Satelite>().Shooted();
        }

    }

    void Take()
    {
        Instantiate(takeEffect, transform.position, Quaternion.identity);
        CastFlash();
        Destroy(gameObject);
    }
}
