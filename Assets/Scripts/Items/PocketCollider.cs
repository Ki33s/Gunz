using UnityEngine;

public class PocketCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.transform.tag == "Bonus")
        {
            myTrigger.transform.SendMessage("Take");
        }
    }
}
