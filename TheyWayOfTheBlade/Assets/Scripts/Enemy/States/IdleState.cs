using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class IdleState : States
    {
        public PursueTargetState pursueTargetState;

        public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator)
        {
            #region Handle Enemy Target Detection
            //Look for a potential Target
            //Switch to the pursue target state if target is found
            //If not return this state

            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, enemyManager.detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharactersStats charactersStats = colliders[i].transform.GetComponent<CharactersStats>();

                if (charactersStats != null)
                {
                    //enemyManager.targetDirection = charactersStats.transform.position - transform.position;
                    //float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (enemyManager.viewableAngle > enemyManager.minDetectionAngle && enemyManager.viewableAngle < enemyManager.maxDetectionAngle)
                    {
                        enemyManager.currentTarget = charactersStats;
                    }
                }
            }
            #endregion

            #region Handle SWitching To Next State
            //If we have target change state else remain here
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}