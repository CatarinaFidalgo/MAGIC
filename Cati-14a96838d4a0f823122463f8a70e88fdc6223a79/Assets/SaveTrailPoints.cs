using GK;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SaveTrailPoints : MonoBehaviour
{
    public Transform trail;
    private int i = 0;
    public List<Vector3> pointsTrail = new List<Vector3>();
    public bool pressed = false;
    public ConvexHullTry convexHullTry;
    public Renderer rend;
    public TrailRenderer trailRend;
    //private Vector3 mousePosition;
    //public GameObject spherePoint;
    public SendTrail sendTrail;
    public StartEndLogs startEnd;

    /* Requirements for buttons to work:
        - Include an instance of OVRManager anywhere in your scene. 
        - Call OVRInput.Update() once per frame at the beginning of any component’s Update methods.
     
    */

    void LateUpdate()
    {
        OVRInput.Update();

        if ((OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three)) && startEnd.showWorkspace)
        {    
            //Activate trail renderer

            rend.enabled = true;

            pointsTrail.Add(trail.position); // save trail positions in the points list for the CH



            //Debug.Log("Entry number" + i + ": " + pointsTrail[i]);
            //Instantiate(spherePoint, pointsTrail[i], Quaternion.identity);
            i++;

            //Change flags
            pressed = true;
            convexHullTry.generateHullDone = false;
            sendTrail.sent = false;
        }

        else
        {
            i = 0;
            trailRend.Clear(); //Clear past trails
            
            //Change flags
            pressed = false;            
            rend.enabled = false;
            

            //Debug.Log("OUTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");

        }
        /*
                if (!OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger))
                {
                    Debug.Log("Is pointing Right");
                }
                else
                {
                    Debug.Log("Not pointing Right");
                }

                if (!OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger))
                {
                    Debug.Log("Is pointing Left");
                }

                else
                {
                    Debug.Log("Not pointing Left");
                }
                
        Debug.Log("Right: " + OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
        Debug.Log("Left: " + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
        //Debug.Log("Rest: " + OVRInput.Get(OVRInput.Touch.SecondaryThumbRest));
        Debug.Log("B: " + OVRInput.Get(OVRInput.Button.Two));
        Debug.Log("A: " + OVRInput.Get(OVRInput.Button.One));*/

    }
}
