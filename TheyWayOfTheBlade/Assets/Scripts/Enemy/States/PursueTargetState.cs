using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class PursueTargetState : States
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;

        public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator)
        {
            //Chase the  target
            //If within attack range , switch to combat state
            //If target is out of range, return this state and continue to chase target

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.viewableAngle > 65 || enemyManager.viewableAngle < -65)
            {
                return rotateTowardsTargetState;
            }

            if (enemyManager.isInteracting)
                return this;

            if (enemyManager.isPerformingAction)
            {
                enemyManager.enemyAnimator.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (enemyManager.distanceFromTarget > enemyManager.maxAggroRadius && enemyManager.currentRecoveryTime <= 0)
            {
                enemyManager.enemyAnimator.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (enemyManager.distanceFromTarget <= enemyManager.maxAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }


        void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            //Rotate manually
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
                enemyManager.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            // Rotate with navmesh
            else
            {
                Vector3 targetVelocity = enemyManager.rb.velocity;

                enemyManager.navMeshAgent.enabled = true;

                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                enemyManager.rb.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Lerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

    }
}