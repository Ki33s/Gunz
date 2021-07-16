using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoney : MonoBehaviour
{
    public TextMesh thisText;
    public float transp = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transp > 0)
        {
            transp -= Time.deltaTime * 85;
            thisText.color = new Color32(255, 0, 0, (byte)transp);
        }       
    }
}
