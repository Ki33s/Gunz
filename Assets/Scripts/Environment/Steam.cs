using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : MonoBehaviour
{
    public int force = 500;
    public AudioSource steamSound;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
        steamSound.Play();
    }

   /* void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun" || myTrigger.gameObject.tag == "Enemy")
        {
            myTrigger.GetComponent<Rigidbody>().AddForce(-transform.TransformDirection(Vector3.left) * force, ForceMode.Force);
        }
    }*/
    
}
