using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class DamageCollider : MonoBehaviour
    {
        PlayerStats playerStats;

        //public int weaponDamage = 1;
        [HideInInspector]public bool canDoDamage = false;


        private void Start()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            canDoDamage = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                if (canDoDamage == true)
                {
                    EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                    if (enemyStats != null)
                    {
                        canDoDamage = false;
                        enemyStats.TakeDamage(playerStats.weaponDamage);
                    }
                }
            }

            if (other.tag == "Dummy")
            {
                if (canDoDamage == true)
                {
                    Dummy dummy = other.GetComponent<Dummy>();

                    if (dummy != null)
                    {
                        canDoDamage = false;
                        dummy.DummyTakeDamage();
                    }
                }
            }
        }
    }
}