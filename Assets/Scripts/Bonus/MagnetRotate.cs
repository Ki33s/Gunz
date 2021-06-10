using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRotate : MonoBehaviour
{
    public int rotateSpeed = 360;

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
