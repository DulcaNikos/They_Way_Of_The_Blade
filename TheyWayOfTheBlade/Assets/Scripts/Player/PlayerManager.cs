using UnityEngine;

namespace Tsushima
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerCamera playerCamera;

        private PlayerMovement playerMovement;

        [HideInInspector] public PlayerAnimator playerAnimator;

        [HideInInspector] public CharacterController characterController;

        [HideInInspector] public PlayerStats playerStats;

        [HideInInspector] DamageCollider damageCollider;

        public bool isGrounded = false;


        // Start is called before the first frame update
        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>();

            characterController = GetComponent<CharacterController>();

            playerStats = GetComponent<PlayerStats>();
            damageCollider = GetComponentInChildren<DamageCollider>();
        }

        private void Start()
        {
            isGrounded = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (playerStats.isDead) return;

            if (playerAnimator.animator.GetBool("isInteracting"))
                return;

            //Handle Player Movement
            playerMovement.HandleGravity();
        }

        private void LateUpdate()
        {
            if (playerStats.isDead) return;

            //Handle Camera Follow
            playerCamera.HandleAllCameraActions();
        }

        public void EnableCanDoDamage()
        {
            damageCollider.canDoDamage = true;
        }

        public void DisableCanDoDamage()
        {
            damageCollider.canDoDamage = false;
        }
    }
}