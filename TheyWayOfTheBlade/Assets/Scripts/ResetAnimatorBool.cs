using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isInteracting = "isInteracting";
    public bool isInteractingStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    //For the enemy
    public string isRotatingWithRootMotionBool = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteracting, isInteractingStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus); 
    }
}
