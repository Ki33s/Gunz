using UnityEngine;

public class BulletShieldCollider : MonoBehaviour
{
    public int force = 400;
    public int delay = 5;
    public Color readyColor;
    public Color unreadyColor;
    private float delayTimer = 0;
    private bool isReady = true;

    private void FixedUpdate()
    {
        if(!isReady)
        {
            if (delayTimer < delay)
                delayTimer += Time.deltaTime;
            else
                Ready();
        }
    }
    void OnTriggerEnter(Collider myTrigger)
    {
        if (isReady)
        {
            if (myTrigger.gameObject.tag == "Enemy")
            {
                myTrigger.transform.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Force);
                myTrigger.transform.SendMessage("Shooted");
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
                Unready();
            }
            else if (myTrigger.gameObject.tag == "DestroyableEnemy")
            {
                myTrigger.transform.SendMessage("Shooted");
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().Kills++;
                GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateKills();
                Unready();
            }
            else if (myTrigger.transform.tag == "Aircoin")
            {
                myTrigger.transform.GetComponent<Aircoin>().Shooted();
            }
            else if (myTrigger.transform.tag == "Satelite")
            {
                if (!myTrigger.transform.GetComponent<Satelite>().dead)
                    myTrigger.transform.GetComponent<Satelite>().Shooted();
            }
        }
    }

    private void Unready()
    {
        isReady = false;
        GetComponent<Outline>().OutlineColor = unreadyColor;
    }

    private void Ready()
    {
        isReady = true;
        delayTimer = 0;
        GetComponent<Outline>().OutlineColor = readyColor;
    }
}
