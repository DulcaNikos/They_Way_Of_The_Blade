using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class RotateTowardsTargetState : States
    {
        public CombatStanceState combatStanceState;

        public override States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator)
        {
            enemyAnimator.animator.SetFloat("Vertical",0);
            enemyAnimator.animator.SetFloat("Horizontal", 0);

            if (enemyManager.isInteracting)
                return this;

            if (enemyManager.viewableAngle >= 100 && enemyManager.viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                enemyAnimator.PlayeTargetAnimationWithRootRotation("TurnBack", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle <= -101 && enemyManager.viewableAngle >= -180 && !enemyManager.isInteracting)
            {
                enemyAnimator.PlayeTargetAnimationWithRootRotation("TurnBack", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle <= -45 && enemyManager.viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyAnimator.PlayeTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (enemyManager.viewableAngle >= 45 && enemyManager.viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyAnimator.PlayeTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}