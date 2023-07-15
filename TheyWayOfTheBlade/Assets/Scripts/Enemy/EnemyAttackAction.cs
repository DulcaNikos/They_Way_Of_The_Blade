using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyActions
    {
        //public int attackScore = 3;
        //The time that needs to recover after he attacks
        public float recoveryTime = 2;

        public float maxAttackAngle = 35;
        public float minAttackAngle = -35;

        public float minDinstanceNeededToAttack = 0;
        public float maxDinstanceNeededToAttack = 3;
    }
}