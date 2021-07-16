using UnityEngine;

public class BulletShield : MonoBehaviour
{
    public int rotateSpeed = 180;
    private GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tempgun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            gun = tempgun;
        }

        if (Random.Range(0, 2) == 1)
            rotateSpeed = -rotateSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = gun.transform.position;
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
