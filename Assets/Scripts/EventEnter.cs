using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnter : MonoBehaviour
{
    private GameObject gun;
    public GameObject gunPlace;

    public int damping = 3;
    public float exitTimer = 10f;

    private bool gunEntered = false;
    private bool gunExited = false;

    private void FixedUpdate()
    {
        if(gunEntered)
        {
            if(Mathf.Abs((gun.transform.position - transform.position).magnitude) > 0.1f)
            {
                gun.transform.position = Vector3.Lerp(gun.transform.position, gunPlace.transform.position, Time.deltaTime * damping);
            }

            if (Mathf.Abs(gun.transform.eulerAngles.x - 90) > 0.1f)
            {
                //gun.transform.eulerAngles = Vector3.Lerp(gun.transform.eulerAngles, new Vector3(90, gun.transform.eulerAngles.y, 0), Time.deltaTime);
            }

            CheckTimer();
        }

        if(gunExited)
        {
            if (gun.transform.position.z != -3)
            {
                gun.transform.position = Vector3.Lerp(gun.transform.position, new Vector3(gun.transform.position.x, gun.transform.position.y, -3), Time.deltaTime * damping);
            }
        }
    }

    void CheckTimer()
    {
        if (exitTimer >= 0)
            exitTimer -= Time.deltaTime;
        else
            GameObject.Find("Main Camera").GetComponent<BuildingGenerator>().ExitEvent();
    }

    public void GunExit()
    {
        gunEntered = false;
        gunExited = true;
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" && !gunExited)
        {
            gun = myTrigger.gameObject;
            gunEntered = true;
            gun.GetComponent<Gun>().isEvent = true;
            gun.GetComponent<Rigidbody>().useGravity = false;
            gun.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            GameObject.Find("Main Camera").GetComponent<BuildingGenerator>().isEvent = true;
            GameObject.Find("Main Camera").GetComponent<BuildingGenerator>().lastEventEnter = gameObject;
            GameObject.Find("Main Camera").GetComponent<BuildingGenerator>().eventCamPlace = new Vector3(0, gunPlace.transform.position.y, -10);
        }
    }
}
