using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Match the VR target to the headset etc


public class VRRig2: MonoBehaviour
{
   
    public Transform headVrTarget;
    public Transform headRigTarget;

    public Transform rightHandVrTarget;
    public Transform rightHandRigTarget;

    public Transform leftHandVrTarget;
    public Transform leftHandRigTarget;

    public Transform headConstraint;

    public Vector3 headBodyOffset;
    public Vector3 headTrackingPositionOffset;
    public Vector3 headTrackingRotationOffset;
    public Vector3 rightHandTrackingPositionOffset;
    public Vector3 leftHandTrackingPositionOffset;

    public bool remote = false;
    public Evaluation evaluation;

    void Start()
    {
        //initial diference in position between the head and the body
        headBodyOffset = transform.position - headConstraint.position;
    }

    void Update()
    {
        if (remote && evaluation.condition == ConditionType.Approach)
        {
            // Fazer a approach - WorkspaceTransformation
        }
        else
        {
            updateRig();
        }
    }

    public void updateRig()
    {
        //update da posiçao do avatar
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane( headVrTarget.forward, Vector3.up).normalized;

        //Match entre o target no esqueleto (RigTarget) e os dados vindos dos controladores (VrTarget)
        
        headRigTarget.position = headVrTarget.TransformPoint(headTrackingPositionOffset);
        headRigTarget.rotation = headVrTarget.rotation * Quaternion.Euler(headTrackingRotationOffset);

        
        rightHandRigTarget.position = rightHandVrTarget.TransformPoint(rightHandTrackingPositionOffset);
        rightHandRigTarget.LookAt(rightHandRigTarget.position - rightHandVrTarget.right, rightHandVrTarget.forward);

        
        leftHandRigTarget.position = leftHandVrTarget.TransformPoint(leftHandTrackingPositionOffset);
        leftHandRigTarget.LookAt(leftHandRigTarget.position - (-leftHandVrTarget.right), leftHandVrTarget.forward);


    }
}
