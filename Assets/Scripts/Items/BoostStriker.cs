using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostStriker : MonoBehaviour
{
    public int boostForce = 2000;
    private GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }
        gun.GetComponent<Rigidbody>().AddForce(new Vector3(0, boostForce, 0), ForceMode.Force);
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gun.transform.position;
    }
}
