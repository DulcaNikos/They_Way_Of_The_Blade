using UnityEngine;
using static UnityEngine.UI.Image;

namespace Tsushima
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerManager playerManager;

        PlayerWeapon playerWeapon;

        //We take the amounts from PlayerInputManager script
        private float verticalMovement;
        private float horizontalMovement;

        [Header("Movement Settings")]
        private float moveAmount;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;
        float nextStep = 0f;
        float stepRate = 0.15f;

        [Header("Ground Detection")]
        public float gravity = 9.81f;
        public float groundCheckDistance = 0.2f;
        public float edgeForce = 2f;
        public Transform StartPointOfRaycast;
        public LayerMask groundLayer;


        void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerWeapon = GetComponent<PlayerWeapon>();
        }

        public void HundleAllMovement()
        {
            HandleMovement();
            HandleRotation();
        }

        private void GetMovementInputs()
        {
            verticalMovement = Input.GetAxisRaw("Vertical");
            horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        public void HandleGravity()
        {
            playerManager.isGrounded = CheckGrounded();

            if (playerManager.isGrounded)
            {
                HundleAllMovement();
            }
            else
            {
                // Apply gravity when in the air
                ApplyGravity();
            }
        }

        public bool CheckGrounded()
        {
            // Perform the raycast and check if it hits any ground
            if (Physics.Raycast(StartPointOfRaycast.position, Vector3.down, groundCheckDistance, groundLayer))
            {
                return true;
            }

            return false;
        }

        public void ApplyGravity()
        {
            Vector3 gravityVector = Vector3.down * gravity;
            gravityVector += transform.forward * edgeForce;
            playerManager.characterController.Move(gravityVector * Time.deltaTime);
        }

        void HandleMovement()
        {
            GetMovementInputs();

            //Our move direction is based on our cameras facing perspective & our movement inputs
            moveDirection = playerManager.playerCamera.transform.forward * verticalMovement;
            moveDirection = moveDirection + playerManager.playerCamera.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveAmount = 0;

            //Move actual the player
            if (!Input.GetKey(KeyCode.LeftShift) && playerWeapon.isGuarding == false)
            {
                playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

                if (playerManager.characterController.velocity.magnitude >= 0.1)
                {
                    moveAmount = 0.5f;
                }
            }
            else
            {
                playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);

                if (playerManager.characterController.velocity.magnitude >= 0.51)
                {
                    moveAmount = 1f;
                }
            }

            //Animate the movement
            playerManager.playerAnimator.UpdateAnimatorMovementParameters(moveAmount);
        }

        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = playerManager.playerCamera.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + playerManager.playerCamera.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        void FootSteps()
        {
            if(Time.time > nextStep & moveDirection.magnitude > 0.1f)
            {
                SoundManager.instance.PlayArmorSound();
                nextStep = Time.time + stepRate;
            }
        }
    }
}