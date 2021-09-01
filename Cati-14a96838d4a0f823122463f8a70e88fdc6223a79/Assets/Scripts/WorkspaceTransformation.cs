using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVRTouchSample;
using UnityEngine.SceneManagement;

public class WorkspaceTransformation : MonoBehaviour
{
    //private bool inWorkspace;
    public BoxCollider volumeCollider;
    [Space(8)]
    public Transform warpedSpace;
    [Space(8)]
    public Evaluation evaluation;
    [Space(8)]
    public Transform remoteAvatar;
    [Space(8)]
    public Transform remoteHMD;
    public Transform warpedHMD;
    [Space(8)]
    public Transform remoteLeft;
    public Transform warpedHandLeft;
    [Space(8)]
    public Transform remoteRight;
    public Transform warpedHandRight;
    [Space(8)]
    public Transform remoteFingertipRight;
    public Transform warpedFingertipRight;
    [Space(8)]
    public Transform remoteFingertipLeft;
    public Transform warpedFingertipLeft;    
    [Space(8)]
    public Transform targetLeftTip; // targetTips are the tips of the local avatar hands calculated from the remote hands
    public Transform targetRightTip;    
    [Space(8)]
    public Transform pointingHandTipRemote;
    public SaveTrailPoints saveTrails;
    

    //values for internal use
    private Vector3 transformLeftHandVector;
    private Vector3 transformRightHandVector;
    private float m;
    private float b;
    public bool isPointingRight;
    public bool isPointingRightRemotely;
    //public bool isPointingLeft;
    //public bool isPointingLeftRemotely;
    private float offset_positionZ;

    private void Start()
    {
        m = 0.823529412f;
        b = -0.26f;
    }

    void Update()
    {
        //inWorkspace = volumeCollider.bounds.Contains(remoteFingertipRight.position) || volumeCollider.bounds.Contains(remoteFingertipLeft.position);
                

        if (evaluation.condition == ConditionType.Approach)
        {
            //Mirror workspace
            warpedSpace.localScale = new Vector3(-1, 1, 1);

            /*************** Selection of the pointing hand ****************/

            //Assumes the pointing hand is the one that is further ahead
            
            if (!saveTrails.pressed)
            {
                if (targetRightTip.position.z >= targetLeftTip.position.z) //pointing hand is the RIGHT hand
                {
                    isPointingRight = true;
                    isPointingRightRemotely = false;
                }
                else  //pointing hand is the LEFT hand
                {
                    isPointingRight = false;
                    isPointingRightRemotely = true;
                }
            }

            if (isPointingRight)
            {
                pointingHandTipRemote.position = targetRightTip.position;
                pointingHandTipRemote.rotation = targetRightTip.rotation;
            }

            else
            {
                pointingHandTipRemote.position = targetLeftTip.position;
                pointingHandTipRemote.rotation = targetLeftTip.rotation;
            }
            

            /*************** Calculate warped positions and orientations of head and hands ****************/

            //warpedHMD.localPosition = remoteHMD.localPosition;  //Update head position in (*1*)
            warpedHandRight.localPosition = remoteLeft.localPosition;
            warpedHandLeft.localPosition = remoteRight.localPosition;
            warpedFingertipRight.localPosition = remoteFingertipLeft.localPosition;
            warpedFingertipLeft.localPosition = remoteFingertipRight.localPosition;


            warpedHMD.localRotation = remoteHMD.localRotation;
            warpedHandRight.localRotation = remoteLeft.localRotation;
            warpedHandLeft.localRotation = remoteRight.localRotation;
            warpedFingertipRight.localRotation = remoteFingertipLeft.localRotation;
            warpedFingertipLeft.localRotation = remoteFingertipRight.localRotation;

            // (*1*)
            /*****************************    Head position update     **************************/

            // Moves head according to the position of the local hand tip along the equation:
            //           hmd_z = m * (-pointingHandTipRemote.position.z) + b

            warpedHMD.localPosition = new Vector3(remoteHMD.localPosition.x, remoteHMD.localPosition.y, m * (-pointingHandTipRemote.position.z) + b);
            offset_positionZ = remoteHMD.localPosition.z - warpedHMD.localPosition.z;
            //Debug.Log(offset_positionZ);

            /*************** Make remote hands point to the same place as local avatar ****************/
            /**************       (Only the pointing hands suffers this update)     ****************/

            //Calculate the local tips from the remote tip coordinates
            targetRightTip.localPosition = new Vector3(-warpedFingertipLeft.localPosition.x, warpedFingertipLeft.localPosition.y, -warpedFingertipLeft.localPosition.z); //local right fingertip from the left warped hand
            targetLeftTip.localPosition = new Vector3(-warpedFingertipRight.localPosition.x, warpedFingertipRight.localPosition.y, -warpedFingertipRight.localPosition.z); //local left fingertip from the right warped hand

            //transformHandVector is the transform vector from the remote to the local tip
            //our Right and Left transform vectors are a function the Left and Right tips, respectively (Changed because of the mirroring)
            transformRightHandVector = targetLeftTip.position - warpedFingertipRight.position; 
            transformLeftHandVector = targetRightTip.position - warpedFingertipLeft.position;

            //Update of the pointing warped hand position with the transform vector
            if (isPointingRight) 
            {
                warpedHandLeft.position += transformLeftHandVector;
                warpedHandRight.position = new Vector3 (warpedHandRight.position.x, warpedHandRight.position.y, warpedHandRight.position.z + offset_positionZ);
            }

            if (!isPointingRight)
            {
                warpedHandRight.position += transformRightHandVector;
                warpedHandLeft.position = new Vector3(warpedHandLeft.position.x, warpedHandLeft.position.y, warpedHandLeft.position.z + offset_positionZ);
            }  
        }       

        if (evaluation.condition == ConditionType.Veridical)
        {
            //Warped hands are the same as the remote hands received through the network

            warpedSpace.localScale = new Vector3(1, 1, 1);

            warpedHMD.localPosition = remoteHMD.localPosition;
            warpedHandLeft.localPosition = remoteLeft.localPosition;
            warpedHandRight.localPosition = remoteRight.localPosition;
            warpedFingertipRight.localPosition = remoteFingertipLeft.localPosition;
            warpedFingertipLeft.localPosition = remoteFingertipRight.localPosition;


            warpedHMD.localRotation = remoteHMD.localRotation;            
            warpedHandLeft.localRotation = remoteLeft.localRotation;            
            warpedHandRight.localRotation = remoteRight.localRotation;
            warpedFingertipRight.localRotation = remoteFingertipLeft.localRotation;
            warpedFingertipLeft.localRotation = remoteFingertipRight.localRotation;


            /*************** Selection of the pointing hand ****************/

            if (!saveTrails.pressed)
            {
                //Assumes the pointing hand is the one that is further ahead
                if (warpedFingertipRight.position.z >= warpedFingertipLeft.position.z) //pointing hand is the RIGHT hand
                {
                    isPointingRight = true;
                    isPointingRightRemotely = true;

                }
                else if (warpedFingertipRight.position.z < warpedFingertipLeft.position.z) //pointing hand is the LEFT hand
                {
                    isPointingRight = false;
                    isPointingRightRemotely = false;
                }
            }

            
            if (isPointingRight)
            {
                pointingHandTipRemote.position = warpedFingertipLeft.position;
                pointingHandTipRemote.rotation = warpedFingertipLeft.rotation;
            }

            else
            {
                pointingHandTipRemote.position = warpedFingertipRight.position;
                pointingHandTipRemote.rotation = warpedFingertipRight.rotation;
            }
                      

        }
    }

}

