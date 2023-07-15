using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class CombatStanceState : States
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public EnemyAttackAction[] enemyAttacks;

        float verticalMovementValue = 0;
        float horizontalMovementValue = 0;


        bool randomDestinationSet = false;

        public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator)
        {
            enemyAnimator.animator.SetFloat("Vertical",verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimator.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimator.animator.SetFloat("Vertical", 0);
                enemyAnimator.animator.SetFloat("Horizontal", 0);
                return this;
            }

            if (enemyManager.distanceFromTarget > enemyManager.maxAggroRadius)
            {
                return pursueTargetState;
            }

            if(!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCircleAction();
            }

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return this;
        }

        void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            ////Rotate manually
            if (enemyManager.isPerformingAction)
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
            //Rotate with navmesh
            else
            {
                Vector3 targetVelocity = enemyManager.rb.velocity;

                enemyManager.navMeshAgent.enabled = true;

                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                enemyManager.rb.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void DecideCircleAction()
        {
            WalkAroundTarget();
        }

        private void WalkAroundTarget()
        {
            verticalMovementValue = -0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        //Handle which attack will be
        void GetNewAttack(EnemyManager enemyManager)
        {
            //int maxScore = 0;

            //for (int i = 0; i < enemyAttacks.Length; i++)
            //{
            //    EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            //    if (dinstanceFromTarget <= enemyAttackAction.maxDinstanceNeededToAttack
            //        && dinstanceFromTarget >= enemyAttackAction.minDinstanceNeededToAttack)
            //    {
            //        if (viewableAngle <= enemyAttackAction.maxAttackAngle
            //            && viewableAngle >= enemyAttackAction.minAttackAngle)
            //        {
            //            maxScore += enemyAttackAction.attackScore;
            //        }
            //    }
            //}

            //int randomValue = Random.Range(0, maxScore);
            //int temporaryScore = 0;

            //for (int i = 0; i < enemyAttacks.Length; i++)
            //{
            //    EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            //    if (dinstanceFromTarget <= enemyAttackAction.maxDinstanceNeededToAttack
            //        && dinstanceFromTarget >= enemyAttackAction.minDinstanceNeededToAttack)
            //    {
            //        if (viewableAngle <= enemyAttackAction.maxAttackAngle
            //            && viewableAngle >= enemyAttackAction.minAttackAngle)
            //        {
            //            if (attackState.currentAttack != null)
            //                return;

            //            temporaryScore += enemyAttackAction.attackScore;

            //            if (temporaryScore > randomValue)
            //            {
            //                attackState.currentAttack = enemyAttackAction;
            //            }
            //        }
            //    }
            //}


            int randomValue = Random.Range(0, enemyAttacks.Length);

            EnemyAttackAction enemyAttackAction = enemyAttacks[randomValue];

            if (enemyManager.distanceFromTarget <= enemyAttackAction.maxDinstanceNeededToAttack
                && enemyManager.distanceFromTarget >= enemyAttackAction.minDinstanceNeededToAttack)
            {
                if (enemyManager.viewableAngle <= enemyAttackAction.maxAttackAngle
                    && enemyManager.viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    if (attackState.currentAttack != null)
                        return;

                    attackState.currentAttack = enemyAttackAction;
                }
            }
        }
    }
}