using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunZRotate : MonoBehaviour
{
    public bool right = false;
    public int rotateSpeed = 270;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            right = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
     if (right)
         transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
     else
        transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void ChangeRotate()
    {
        if (right)
                right = false;
            else
                right = true;
    }
}
