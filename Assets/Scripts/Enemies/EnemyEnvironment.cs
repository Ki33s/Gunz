using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnvironment : MonoBehaviour
{
    public int steamRange = 3;

    public float steamTimer = 0;
    public bool steamRight;

    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (steamTimer > 0)
            Steam();
    }

    void Steam()
    {
        float deltaX = steamRange * steamTimer * Time.deltaTime;

        if (!steamRight)
            deltaX *= -1;

        transform.position = new Vector3(transform.position.x + deltaX, transform.position.y, transform.position.z);

        steamTimer -= Time.deltaTime;
    }
}
