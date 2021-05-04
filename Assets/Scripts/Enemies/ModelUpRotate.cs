using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelUpRotate : MonoBehaviour
{
    public int rotateSpeed = 720;
    private bool right = true;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 1)
            right = false;
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
            transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime);

        }
        else
        {
            transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
        }
    }
}
