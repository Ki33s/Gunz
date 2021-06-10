using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public int boostForce = 2000;
    public AudioSource takeBoost;

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            if(!myTrigger.GetComponent<Gun>().isEvent)
            {
                takeBoost.Play();
                myTrigger.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0) * boostForce, ForceMode.Force);
            }
            
        }
    }
}
