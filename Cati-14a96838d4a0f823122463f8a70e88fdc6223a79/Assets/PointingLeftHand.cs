using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingLeftHand : MonoBehaviour
{
    //public bool inWorkspace;
    public bool inWorkspace;

    public Transform fingerTip;

    private Transform fingerPart1;
    private Transform fingerPart2;
    private Transform fingerPart3;

    public BoxCollider volumeCollider;

    //public WorkspaceTransformation wsTransformation;

    public PointingFingerLocal p;

    private int i = 0;

    void Update()
    {        
        
        //Check if the parent transform (Right Hand) is inside the box collider (volumeCollider)
        inWorkspace = volumeCollider.bounds.Contains(fingerTip.position);
        
        //Changes the rotation of the fingers for a pointing/relaxed position

        for (i = 0; i <= 4; i++)
        {

            //Assigns the transform of the first child of the Game Object this script is attached to.
            fingerPart1 = this.gameObject.transform.GetChild(i);
            fingerPart2 = this.gameObject.transform.GetChild(i).GetChild(0);
            fingerPart3 = this.gameObject.transform.GetChild(i).GetChild(0).GetChild(0);

            if (inWorkspace == true && p.isPointingRight == false )  // Fingers in Pointing Position
            {
                //Debug.Log("In Workspace");
                switch (i)
                {
                    case 0: //index
                        /*fingerPart1.localRotation = Quaternion.Euler(-1.038f, -0.096f, 4.552f);
                        fingerPart2.localRotation = Quaternion.Euler(0f, 0.004f, -0.351f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, 0f, 0.013f);  */                     
                        break;

                    case 1: //middle
                        fingerPart1.localRotation = Quaternion.Euler(54.66f, -0.139f, 0.601f);
                        fingerPart2.localRotation = Quaternion.Euler(104f, 0.002f, -0.5100001f);
                        fingerPart3.localRotation = Quaternion.Euler(36.79f, 0.008f, 0.194f);
                        break;

                    case 2: //pinky
                        fingerPart1.localRotation = Quaternion.Euler(71.12f, -0.905f, 4.523f);
                        fingerPart2.localRotation = Quaternion.Euler(90.3f, 0.002f, -0.325f);
                        fingerPart3.localRotation = Quaternion.Euler(39.67f, 0.004f, 0.0730f);                        
                        break;

                    case 3: //ring
                        fingerPart1.localRotation = Quaternion.Euler(65.93f, -0.659f, 4.418f);
                        fingerPart2.localRotation = Quaternion.Euler(98.1f, -0.004f, -0.241f);
                        fingerPart3.localRotation = Quaternion.Euler(42.18f, 0.003f, 0.265f);
                        break;

                    case 4: //thumb
                        fingerPart1.localRotation = Quaternion.Euler(27.71f, -55f, -23.3f);
                        fingerPart2.localRotation = Quaternion.Euler(0.06f, 0.574f, -5.049f);
                        fingerPart3.localRotation = Quaternion.Euler(0.005f, 0.436f, -9.783f);
                        break;
           
                    default:
                        Debug.Log("ERROR");
                        break;
                }
            }

            else if (inWorkspace == false || p.isPointingRight == true) 
            {

                // Fingers in Relaxed Position - out of the workspace or if the hand isn't pointing 
                //Debug.Log("NOOOOOOOOOOT Workspace");

                switch (i)
                {
                    case 0: //index
                        /*fingerPart1.localRotation = Quaternion.Euler(-1.457f, -0.097f, 5.167f);
                        fingerPart2.localRotation = Quaternion.Euler(0f, 0.004f, 0.351f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, 0f, -0.013f);*/
                        break;

                    case 1: //middle
                        fingerPart1.localRotation = Quaternion.Euler(-1.442f, -0.275f, 4.936f);
                        fingerPart2.localRotation = Quaternion.Euler(0f, -0.001f, -0.351f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, 0f, -0.013f);
                        break;

                    case 2: //pinky
                        fingerPart1.localRotation = Quaternion.Euler(-1.381f, -1.002f, 4.965f);
                        fingerPart2.localRotation = Quaternion.Euler(0f, 0.004f, -0.252f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, -0.003f, -0.084f);
                        break;

                    case 3: //ring
                        fingerPart1.localRotation = Quaternion.Euler(-1.384f, -0.97f, 4.649f);
                        fingerPart2.localRotation = Quaternion.Euler(0f, -0.002f, 0.213f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, 0f, 0.008f);
                        break;

                    case 4: //thumb
                        fingerPart1.localRotation = Quaternion.Euler(21.975f, -14.766f, -30.491f);
                        fingerPart2.localRotation = Quaternion.Euler(0.055f, 0.539f, -5.208f);
                        fingerPart3.localRotation = Quaternion.Euler(0f, 0.003f, -9.046f);
                        break;
                        
                    default:
                        Debug.Log("ERROR");
                        break;
                }

            }
        }
    }
}

