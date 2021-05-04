using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRotation : MonoBehaviour
{

    public int rotateSpeed = 360;

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
