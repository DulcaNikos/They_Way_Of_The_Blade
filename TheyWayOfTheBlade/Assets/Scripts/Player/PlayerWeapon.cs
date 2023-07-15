using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsushima
{
    public class PlayerWeapon : MonoBehaviour
    {
        PlayerManager playerManager;

        //public GameObject modelPrefab;
        public MeshRenderer katanaRenderer;
        public BoxCollider katanaCollider;

        public bool isUnarmed;
        public bool isGuarding;

        public GameObject showCanDocombo;

        //
        public string[] attacks;

        public string[] heavyAttacks;

        //
        [HideInInspector]public int currentAttacStep;
        [HideInInspector]public int currentHeavyAttackCompo;


        private void Start()
        {
            currentAttacStep = 0;
            currentHeavyAttackCompo = 0;

            playerManager = GetComponent<PlayerManager>();
            isUnarmed = true;
            isGuarding = false;

            playerManager.playerAnimator.animator.SetBool("isGuarding", false);

            katanaCollider.enabled = false;
            katanaRenderer.enabled = false;
        }

        private void Update()
        {
            if (playerManager.playerStats.isDead) return;

            if (playerManager.playerAnimator.animator.GetBool("isInteracting"))
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isUnarmed == true && playerManager.playerAnimator.animator.GetBool("isUnarmed") == true)
                {
                    //Play load Weapon animation
                    playerManager.playerAnimator.PlayTargetAnimation("Equip_Weapon", true);
                    playerManager.playerAnimator.animator.SetBool("isUnarmed", false);
                }
                else
                {
                    //Play unload Weapon animation
                    playerManager.playerAnimator.PlayTargetAnimation("Unequip_Weapon", true);
                    playerManager.playerAnimator.animator.SetBool("isUnarmed", true);
                    //playerManager.playerAnimator.animator.SetBool("isGuarding", false);
                }
            }

            if (isUnarmed == false)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ExecuteAttackCombo();
                }

                if (Input.GetKey(KeyCode.Mouse1))
                {
                    ExecuteHeavyAttackCombo();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    isGuarding = true;
                }

                if (Input.GetKeyUp(KeyCode.F))
                {
                    isGuarding = false;
                }

                if (isGuarding == true && playerManager.playerAnimator.animator.GetBool("isGuarding") == false)
                {
                    playerManager.playerAnimator.PlayTargetAnimation("Idle_to_Guard", true);
                    playerManager.playerAnimator.animator.SetBool("isGuarding", true);
                }

                if (isGuarding == false && playerManager.playerAnimator.animator.GetBool("isGuarding") == true)
                {
                    playerManager.playerAnimator.PlayTargetAnimation("Guard_to_Idle", true);
                    playerManager.playerAnimator.animator.SetBool("isGuarding", false);
                }
            }
        }

        void ExecuteHeavyAttackCombo()
        {
            //Execute the corresponding action for the completed combo
            switch (currentHeavyAttackCompo)
            {
                case 0:
                    playerManager.playerAnimator.PlayTargetAnimation(heavyAttacks[currentHeavyAttackCompo], true);
                    break;
                case 1:
                    playerManager.playerAnimator.PlayTargetAnimation(heavyAttacks[currentHeavyAttackCompo], true);
                    break;
                default:
                    break;
            }
        }

        void ExecuteAttackCombo()
        {
            //Execute the corresponding action for the completed combo
            switch (currentAttacStep)
            {
                case 0:
                    playerManager.playerAnimator.PlayTargetAnimation(attacks[currentAttacStep], true);
                    break;
                case 1:
                    playerManager.playerAnimator.PlayTargetAnimation(attacks[currentAttacStep], true);
                    break;
                case 2:
                    playerManager.playerAnimator.PlayTargetAnimation(attacks[currentAttacStep], true);
                    break;
                case 3:
                    playerManager.playerAnimator.PlayTargetAnimation(attacks[currentAttacStep], true);
                    break;
                default:
                    break;
            }
        }

        public void UnloadWeaponModel()
        {
            //modelPrefab.SetActive(false);
            katanaCollider.enabled = false;
            katanaRenderer.enabled = false;
            isUnarmed = true;
        }

        public void LoadWeaponModel()
        {
            //modelPrefab.SetActive(true);
            katanaCollider.enabled = true;
            katanaRenderer.enabled = true;
            isUnarmed = false;
        }

        public void CanDoAttackCombo()
        {
            currentAttacStep++;
            showCanDocombo.SetActive(true);
        }

        public void ResetAttackCombo()
        {
            currentAttacStep = 0;
            showCanDocombo.SetActive(false);
        }

        public void CanDoHeavyAttackCombo()
        {
            currentHeavyAttackCompo++;
            showCanDocombo.SetActive(true);
        }

        public void ResetHeavyAttackCombo()
        {
            currentHeavyAttackCompo = 0;
            showCanDocombo.SetActive(false);
        }
        public void PlayWoosh()
        {
            SoundManager.instance.PlayMissSound();
        }
    }
}