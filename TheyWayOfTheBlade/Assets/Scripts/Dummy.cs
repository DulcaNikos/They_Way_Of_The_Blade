using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void DummyTakeDamage()
    {
        SoundManager.instance.PlayWoodHitSound();
        animator.Play("pushed");
    }
}
