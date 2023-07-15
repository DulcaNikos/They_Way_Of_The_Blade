using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class PlayerAnimator : MonoBehaviour
    {
        [HideInInspector] public Animator animator;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }

        public void UpdateAnimatorMovementParameters(float vertical)
        {
            animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        }
    }
}