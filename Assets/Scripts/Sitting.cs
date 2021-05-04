using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sitting : MonoBehaviour
{
    public int animations = 4;
    public float animationTimer;
    private Animator anim;
    private int curAnim;
    private float curTimer;
        
    void Start()
    {
        anim = GetComponent<Animator>();
        ChangeAnimation();
    }

    void FixedUpdate()
    {
        if (curTimer > 0)
            curTimer -= Time.deltaTime;
        else
            ChangeAnimation();
    }

    void ChangeAnimation()
    {
        curAnim = Random.Range(0, animations);
        curTimer = animationTimer;
        anim.SetInteger("Animation", curAnim);
    }

}
