using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]public Animator animator;
    private string animationState;

    //Boolean
    private readonly string isWalking = "isWalking";
    private readonly string isCombat = "isCombat";
    private readonly string isShooting = "isShooting";

    void Start()
    {
        animator = GetComponent<Animator>();
        animationState = "";
    }

    void Update()
    {
        AnimationManager(animationState);
    }

    public void AnimationManager(string state)
    {
        if (state == "Walk") { 
            animator.SetBool(isWalking, true);
            animator.SetBool(isCombat, false);
            animator.SetBool(isShooting, false);
        }
        if (state == "StopWalk")
        {
            animator.SetBool(isWalking, false);
        }
        if (state == "Aim") {
            animator.SetBool(isWalking, false);
            animator.SetBool(isCombat, true);
            animator.SetBool(isShooting, false);
        }
        if (state == "Shoot") {
            animator.SetBool(isShooting, true);
        }
    }
}
