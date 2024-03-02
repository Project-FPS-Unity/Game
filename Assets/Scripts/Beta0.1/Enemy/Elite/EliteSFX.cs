using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSFX : MonoBehaviour
{
    [SerializeField] private AudioSource titanSteps;
    [SerializeField] private AudioSource titanShout;
    [SerializeField] private AudioSource titanAttack;
    [SerializeField] private AudioSource titanSlam;

    public void Shout()
    {
        titanSteps.Stop();
        titanAttack.Stop();
        titanSlam.Stop();
        titanShout.Play();
    }

    public void Step()
    {
        titanSteps.Play();
        titanAttack.Stop();
        titanSlam.Stop();
    }

    public void Attack()
    {
        titanSteps.Stop();
        titanAttack.Play();
    }

    public void Slam()
    {
        titanSteps.Stop();
        titanAttack.Stop();
        titanSlam.Play();
    }
}
