using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircoin : MonoBehaviour
{
    public GameObject steam;
    public GameObject steamPoint;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            Instantiate(steam, steamPoint.transform.position, steamPoint.transform.rotation);
        }
    }

    public void Shooted()
    {
        Instantiate(steam, steamPoint.transform.position, steamPoint.transform.rotation);
    }
}
