using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour
{
    public int force = 500;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            myTrigger.GetComponent<Rigidbody>().AddForce(-transform.TransformDirection(Vector3.left) * force, ForceMode.Force);
        }
        else if (myTrigger.gameObject.tag == "Enemy")
        {
            myTrigger.GetComponent<EnemyEnvironment>().steamTimer = 1;

            if (transform.position.x > 0)
                myTrigger.GetComponent<EnemyEnvironment>().steamRight = false;
            else
                myTrigger.GetComponent<EnemyEnvironment>().steamRight = true;
        }
    }

    
}
