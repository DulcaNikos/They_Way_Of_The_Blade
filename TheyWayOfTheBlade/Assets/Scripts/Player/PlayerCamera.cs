using UnityEngine;

namespace Tsushima
{
    public class PlayerCamera : MonoBehaviour
    {
        public PlayerManager playerManager;

        public Camera cameraObject;

        public Transform cameraPivotTransform;

        //Change these to tweak camera performance
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // The bigger this number, the longer for the camera to reach its position during movement
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; // The lowest point you are able to look down
        [SerializeField] float maximumPivot = 60; // The highest point you are able to look up
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used for camera collisions(moves the camera object to this position upon colliding)
        private float leftAndRightLookAngle;
        private float upAndDownLookAngle;
        private float cameraZPosition; //Values used for camera collisions
        private float targetCameraZPosition; ////Values used for camera collisions
        private float mouseX;
        private float mouseY;

        private void Start()
        {
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();
        }

        private void HandleFollowTarget()
        {
            Vector3 targetcameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetcameraPosition;
        }

        private void GetCameraInputs()
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        private void HandleRotation()
        {
             GetCameraInputs();
 
             //If locked on , force rotation towards target
             //else rotate regulary

             //Rotate left and right based on horizontal 
             leftAndRightLookAngle += (mouseX * leftAndRightRotationSpeed) * Time.deltaTime;
            //Rotate up and down based on vertical 
            upAndDownLookAngle -= (mouseY * upAndDownRotationSpeed) * Time.deltaTime;
            //Clamp the up and down look angle between a min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // Rotate this gameobject left and right
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //Rotate the pivot gameobject up and down
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            //Direction for collision check
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            //We check if there is an object in front of our desired direction ^ (set above)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                //If there is , we get our distance from it
                float dinstanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                //We then equate our target z position to the following
                targetCameraZPosition = -(dinstanceFromHitObject - cameraCollisionRadius);
            }

            //If our target position is less than our collision radius, we substract our collision radius (making it snap back)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            //We then apply our final position using a lerp over a time of 0.2f
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }
}