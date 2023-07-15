using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsushima
{
    public class PlayerStats : CharactersStats
    {
        PlayerManager playerManager;
        PlayerWeapon playerWeapon;

        public PlayerHealthbar healthbar;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            playerWeapon = GetComponent<PlayerWeapon>();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;


            if (playerWeapon.isGuarding == true)
            {
                playerManager.playerAnimator.PlayTargetAnimation("Guard_Hit", true);
                SoundManager.instance.PlaySwordHit();
            }
            else
            {
                currentHealth = currentHealth - damage;

                healthbar.SetCurrentHealth(currentHealth);

                playerManager.playerAnimator.PlayTargetAnimation("Hit", true);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerManager.playerAnimator.animator.Play("Death");
                isDead = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}