using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public abstract class States : MonoBehaviour
    {
        public abstract States Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimator enemyAnimator);
    }
}