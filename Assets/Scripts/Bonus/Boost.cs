using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public int force = 1000;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")//|| myTrigger.gameObject.tag == "Enemy"
        {
            myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * force, ForceMode.Force);
        }
    }
}
