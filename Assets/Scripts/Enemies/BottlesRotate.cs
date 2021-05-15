using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesRotate : MonoBehaviour
{
    public int rotateSpeed = 360;

    public GameObject[] bottles;
    private bool right = false;
    public float gunDeltaY;
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

    public void Elite()
    {
        for (int i = 0; i < bottles.Length; i++)
        {
            bottles[i].GetComponent<Bottle>().Elite();
        }
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
        transform.parent = null;

        for (int i = 0; i < bottles.Length; i++)
        {
            bottles[i].GetComponent<Bottle>().gunDeltaY = gunDeltaY;
            bottles[i].GetComponent<Bottle>().Attack();
        }

        Destroy(gameObject);
    }

    public void Shooted()
    {
        Destroy(gameObject);

        for (int i = 0; i < bottles.Length; i++)
        {
            bottles[i].GetComponent<Bottle>().Shooted();
        }
    }
}
