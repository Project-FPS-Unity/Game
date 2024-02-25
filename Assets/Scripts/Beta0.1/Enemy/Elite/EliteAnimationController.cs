using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteAnimationController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    private string animationState;

    //Boolean
    private readonly string isWalking = "isWalking";
    private readonly string isRunning = "isRunning";
    private readonly string isPlayerFound = "isPlayerFound";
    private readonly string isAttack = "isAttack";
    private readonly string isDamaged = "isDamaged";
    private readonly string isDead = "isDead";

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
        if (state == "Walk")
        {
            animator.SetBool(isWalking, true);
        }
        if (state == "Damaged")
        {
            animator.SetBool(isDamaged, true);
        }
        if (state == "Dead")
        {
            animator.SetBool(isDead, true);
        }
        if (state == "PlayerFound")
        {
            animator.SetBool(isPlayerFound, true);
            animator.SetBool(isRunning, true);
        }
        if (state == "Attack")
        {
            animator.SetBool(isAttack, true);
        }
        if (state == "StopAttack")
        {
            animator.SetBool(isAttack, false);
        }
    }
}
