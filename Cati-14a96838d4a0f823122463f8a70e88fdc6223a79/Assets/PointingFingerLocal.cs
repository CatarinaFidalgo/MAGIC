using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingFingerLocal : MonoBehaviour
{
    public Transform rightTip;
    public Transform leftTip;
    public Transform pointingHandTipLocal;

    public SaveTrailPoints saveTrails;

    public bool isPointingRight;

    void Update()
    {
        if (!saveTrails.pressed)
        {
            //Assumes the pointing hand is the one that is further ahead
            if (rightTip.position.z >= leftTip.position.z) //pointing hand is the RIGHT hand
            {
                isPointingRight = true;
            }
            else  //pointing hand is the LEFT hand
            {
                isPointingRight = false;
            }
        }
        
        if (isPointingRight) 
        {
            pointingHandTipLocal.position = rightTip.position;
            pointingHandTipLocal.rotation = rightTip.rotation;
                
        }
        else  
        {
            pointingHandTipLocal.position = leftTip.position;
            pointingHandTipLocal.rotation = leftTip.rotation;
                
        }


        
    }
}
