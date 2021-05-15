using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleRotate : MonoBehaviour
{
    public int rotateSpeed = 360;
    private bool selfPlay = false;
    private bool right = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (selfPlay)
            Rotate();
    }

    void Rotate()
    {
        if (right)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        }
        else
        {
            transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        }
    }

    public void Attack()
    {
        if (Random.Range(0, 2) == 1)
            right = true;

        selfPlay = true;
    }
}
