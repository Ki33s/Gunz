using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public int boostForce = 2000;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * boostForce, ForceMode.Force);
        }
    }
}
