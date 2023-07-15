using UnityEngine;
using UnityEngine.AI;

namespace Tsushima
{
    public class EnemyManager : MonoBehaviour
    {
        public CharactersStats currentTarget;
        EnemyStats enemyStats;
        public States currentState;

        [HideInInspector] public EnemyAnimator enemyAnimator;

        public float rotationSpeed = 15;
        public float maxAggroRadius = 1.5f;

        [Header("Flags")]
        public bool isPerformingAction;
        public bool isInteracting;
        public bool isRotatingWithRootMotion;
        public bool canRotate;

        [Header("FOV")]
        public float detectionRadius = 20;
        public float maxDetectionAngle = 50;
        public float minDetectionAngle = -50;
        [HideInInspector]public float currentRecoveryTime = 0;

        public LayerMask detectionLayer;

        [Header("Target Information")]
        [HideInInspector]public float distanceFromTarget;
        [HideInInspector]public Vector3 targetsDirection;
        public float viewableAngle;

        [Header("Ground Detection")]
        public float gravity = 9.81f;
        public float groundCheckDistance = 0.2f;
        public float edgeForce = 2f;
        public bool isGrounded = false;
        public Transform startPointOfRaycast;
        public LayerMask groundLayer;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public NavMeshAgent navMeshAgent;

        void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
            enemyAnimator = GetComponentInChildren<EnemyAnimator>();

            rb = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            navMeshAgent.enabled = false;
        }

        void Start()
        {
             rb.isKinematic = false;
             isGrounded = true;
        }

        void Update()
        {
            if (enemyStats.isDead) return;

            if (currentTarget != null)
            {
                if (currentTarget.isDead) return;
            }


            HandleGravity();
            HandleRecoveryTime();
            HandleStateMachine();

            isInteracting = enemyAnimator.animator.GetBool("isInteracting");
            isRotatingWithRootMotion = enemyAnimator.animator.GetBool("isRotatingWithRootMotion");
            canRotate = enemyAnimator.animator.GetBool("canRotate");

            if (currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetsDirection = currentTarget.transform.position - transform.position;
                //viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
                viewableAngle = Vector3.SignedAngle(targetsDirection, transform.forward, Vector3.up);
            }

        }

        private void LateUpdate()
        {
            if (enemyStats.isDead) return;

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        void HandleStateMachine()
        {
            if (currentState != null)
            {
                States nextState = currentState.Tick(this, enemyStats,enemyAnimator);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        void SwitchToNextState(States state)
        {
            currentState = state;
        }

        void HandleRecoveryTime()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        public void HandleGravity()
        {
            isGrounded = CheckGrounded();

            if (!isGrounded)
                ApplyGravity();
        }

        public bool CheckGrounded()
        {
            // Perform the raycast and check if it hits any ground
            if (Physics.Raycast(startPointOfRaycast.position, Vector3.down, groundCheckDistance, groundLayer))
            {
                return true;
            }

            return false;
        }

        public void ApplyGravity()
        {
            rb.AddForce(transform.forward * edgeForce);
            rb.AddForce(Vector3.down * gravity);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Vector3 fovLine1 = Quaternion.AngleAxis(maxDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(minDetectionAngle, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxAggroRadius);

            //
            Vector3 fovLine3 = Quaternion.AngleAxis(-45, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, fovLine3);

            Vector3 fovLine4 = Quaternion.AngleAxis(-100, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, fovLine4);

            Vector3 fovLine5 = Quaternion.AngleAxis(45, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, fovLine5);


            Vector3 fovLine6 = Quaternion.AngleAxis(100, transform.up) * transform.forward * detectionRadius;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, fovLine6);

            //
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.down * groundCheckDistance);
        }
    }
}