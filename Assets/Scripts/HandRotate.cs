using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public int rotateSpeed = 360;
    private bool gameStarted = false;
    private bool right = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            right = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameStarted)
        {
            if(right)
            {
                transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);

                if (transform.rotation.y > - 0.1f)
                    right = false;
            }
            else
            {
                transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

                if (transform.rotation.y < -0.7f)
                    right = true;
            }
            
        }    
            

        if ((Input.GetKey("space") || Input.touchCount > 0) && !gameStarted)
        {
            gameStarted = true;
        }
    }
}
