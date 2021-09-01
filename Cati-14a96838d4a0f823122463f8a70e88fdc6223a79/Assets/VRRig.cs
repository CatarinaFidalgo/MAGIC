using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Match the VR target to the headset etc
[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    
    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap rightHand;
    public VRMap leftHand;
    public Transform headConstraint;
    public Vector3 headBodyOffset; //initial diference in position between the head and the body

    public bool remote = false;
    public Evaluation evaluation;

    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    void Update()
    {
        if (remote && evaluation.condition == ConditionType.Approach)
        {
            // tou a fazer approach olo
        }
        else
        {
            updateRig();
            Debug.Log("Estou nio vr rig antigo");
        }
    }

    public void updateRig()
    {

        transform.position = headConstraint.position + headBodyOffset;
          
        transform.forward = Vector3.ProjectOnPlane(/*headConstraint.up*/ head.vrTarget.forward, Vector3.up).normalized;


        //transform.forward = Camera.main.transform.forward;

        head.Map();

        //rightHand.Map();
        rightHand.rigTarget.position = rightHand.vrTarget.TransformPoint(rightHand.trackingPositionOffset);        
        rightHand.rigTarget.LookAt(rightHand.rigTarget.position - rightHand.vrTarget.right, rightHand.vrTarget.forward);

        //leftHand.Map();
        leftHand.rigTarget.position = leftHand.vrTarget.TransformPoint(leftHand.trackingPositionOffset);
        leftHand.rigTarget.LookAt(leftHand.rigTarget.position - ( - leftHand.vrTarget.right), leftHand.vrTarget.forward);


    }
}
