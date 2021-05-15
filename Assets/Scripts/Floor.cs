using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour
{
    void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.tag == "Gun")
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
