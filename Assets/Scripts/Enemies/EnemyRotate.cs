using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate : MonoBehaviour
{
    public int rotateSpeed = 360;
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
}
