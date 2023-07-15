using System.Collections;
using UnityEngine;

namespace Tsushima
{
    public class EnemyStats : CharactersStats
    {
        //
        public PlayerWeapon playerWeapon;
        EnemyManager enemyManager;

        string currentAnimation;

        public EnemyHealthbar enemyHealthbar;

        public float timer = 5f;

        void Start()
        {
            enemyManager = GetComponent<EnemyManager>();
            currentHealth = maxHealth;
            enemyHealthbar.SetMaxHealth(maxHealth);
        }


        public void TakeDamage(int damage)
        {
            if (isDead)  return;

            if (playerWeapon.currentAttacStep == 0)
                currentAnimation = "Hit1";
            else if (playerWeapon.currentAttacStep == 1)
                currentAnimation = "Hit2";
            else if (playerWeapon.currentAttacStep == 2)
                currentAnimation = "Hit3";
            else if (playerWeapon.currentAttacStep == 3)
                currentAnimation = "Hit4";

            if (playerWeapon.currentHeavyAttackCompo == 0)
                currentAnimation = "Hit2";
            else if (playerWeapon.currentHeavyAttackCompo == 1)
                currentAnimation = "Hit4";

            enemyManager.enemyAnimator.PlayTargetAnimation(currentAnimation, true);

            currentHealth = currentHealth - damage;
            enemyHealthbar.SetHealth(currentHealth);


            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemyManager.enemyAnimator.animator.Play("Enemy Death");
                isDead = true;
                StartCoroutine(EnemyDies());
            }
        }
        //play animation and set the enemy active to false
        IEnumerator EnemyDies()
        {
            yield return new WaitForSeconds(5);
            gameObject.SetActive(false);
        }

    }
}