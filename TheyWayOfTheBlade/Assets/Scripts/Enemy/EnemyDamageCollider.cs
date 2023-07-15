using System.Collections;
using System.Collections.Generic;
using Tsushima;
using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{
    EnemyStats enemyStats;

    EnemyAnimator enemyAnimator;

    //public int weaponDamage = 1;
    [HideInInspector] public bool canDoDamage = false;


    private void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyAnimator = GetComponentInParent<EnemyAnimator>();
        canDoDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (canDoDamage == true)
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(enemyStats.weaponDamage);

                    //Play animation for hit player that isguarding
                    //PlayerWeapon playerWeapon = other.GetComponent<PlayerWeapon>();
                    //if (playerWeapon != null)
                    //{
                    //    if (playerWeapon.isGuarding == true)
                    //    {
                    //        enemyAnimator.PlayTargetAnimation("", true);
                    //    }
                    //}
                    canDoDamage = false;
                }
            }
        }
    }
}