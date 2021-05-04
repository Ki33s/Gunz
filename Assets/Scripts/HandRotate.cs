using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public int rotateSpeed = 360;
    private bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            rotateSpeed = -rotateSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameStarted)
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        if ((Input.GetKey("space") || Input.touchCount > 0) && !gameStarted)
        {
            gameStarted = true;
        }
    }
}
