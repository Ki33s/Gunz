using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnvironment : MonoBehaviour
{
    public int steamRange = 3;

    public float steamSpeed = 0;
    public bool steamRight;

    private Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (steamSpeed > 0)
            Steam();
    }

    void Steam()
    {
        steamSpeed -= steamRange * Time.deltaTime;
        if(steamSpeed > 0)
        {
            float tempSteamSpeed = steamSpeed;

            if (!steamRight)
                tempSteamSpeed *= -1;

            rig.velocity = new Vector3(rig.velocity.x + tempSteamSpeed, rig.velocity.y, 0);
        }
        else
        {

        }

        
    }
}
