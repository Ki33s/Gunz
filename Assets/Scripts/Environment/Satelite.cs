using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satelite : MonoBehaviour
{
    public int force = 400;

    private float minVector = -1f;
    private float maxVector = 1f;
    public bool dead = false;
    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" && !dead)
        {
            Shooted();
        }
    }

    public void Shooted()
    {
        if(!dead)
        {
            dead = true;
            Rigidbody satRig = gameObject.AddComponent<Rigidbody>();
            satRig.AddForce(new Vector3(Random.Range(minVector, maxVector), Random.Range(minVector, maxVector), Random.Range(minVector, maxVector)) * force, ForceMode.Force);
            satRig.AddTorque(new Vector3(Random.Range(minVector, maxVector), Random.Range(minVector, maxVector), Random.Range(minVector, maxVector)) * force, ForceMode.Force);
        }
    }
            
}
