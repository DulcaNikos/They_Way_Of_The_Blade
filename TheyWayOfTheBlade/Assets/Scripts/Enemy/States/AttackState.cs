using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class AttackState : States
    {
        public RotateTowardsTargetState rotateTowardsTargetState;
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;

        public bool hasPerformedAttack;

        public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator)
        {
            RotateTowardsTargetWhileAttacking(enemyManager);

            if(enemyManager.distanceFromTarget > enemyManager.maxAggroRadius)
            {
                return pursueTargetState;
            }

            //Attack?
            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimator,enemyManager);
                currentAttack = null;
            }

            return rotateTowardsTargetState;
        }

        void AttackTarget(EnemyAnimator enemyAnimator,EnemyManager enemyManager)
        {
            enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        void RotateTowardsTargetWhileAttacking(EnemyManager enemyManager)
        {
            //Rotate manually
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                //Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                enemyManager.targetsDirection.y = 0;
                enemyManager.targetsDirection.Normalize();

                if (enemyManager.targetsDirection == Vector3.zero)
                {
                    enemyManager.targetsDirection = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(enemyManager.targetsDirection);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}