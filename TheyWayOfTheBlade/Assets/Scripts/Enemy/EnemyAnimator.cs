using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class EnemyAnimator : MonoBehaviour
    {
        [HideInInspector]public EnemyManager enemyManager;

        [HideInInspector]public Animator animator;

        public EnemyDamageCollider damageCollider;

        private void Start()
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            animator = GetComponent<Animator>();
        }

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }
        public void PlayeTargetAnimationWithRootRotation(string targetAnimation, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }

        public void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.rb.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.rb.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }

        public void EnableCanDoDamage()
        {
            damageCollider.canDoDamage = true;
        }

        public void DisableCanDoDamage()
        {
            damageCollider.canDoDamage = false;
        }

    }
}