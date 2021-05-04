using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    public int force = 10000;
    public GameObject Gun;
    public int ammo;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().Ammo = ammo;
        GameObject.Find("Main Camera").GetComponent<ScoreManager>().updateAmmo();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("space") || Input.touchCount > 0)
        {
            Destroy(gameObject);
            GameObject newGun = Instantiate(Gun, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(transform.eulerAngles.x, 90, 0));
            //newGun.GetComponent<Gun>().GameStart();
            Vector3 forceVector = transform.TransformDirection(Vector3.forward);
            forceVector = new Vector3(forceVector.x, forceVector.y, 0).normalized;
            newGun.GetComponent<Rigidbody>().AddForce(forceVector * force, ForceMode.Force);

            GameObject.Find("Main Camera").GetComponent<BuildingGenerator>().StartGame();
        }
        
    }
}
