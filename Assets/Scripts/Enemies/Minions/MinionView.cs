using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionView : MonoBehaviour
{
    Animator anim; 

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetIdleAnimation()
    {
        anim.SetBool("SetIdle", true);
    }

    public void SetWalkingAnimation()
    {
        anim.SetBool("SetIdle", false);
    }
}
