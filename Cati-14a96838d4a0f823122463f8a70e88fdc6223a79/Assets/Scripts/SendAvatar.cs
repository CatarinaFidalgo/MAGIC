using GK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum MessageSeparators
{
    L1 = '#', // sep transforms
    L2 = '/', // sep propertines 
    L3 = ':' // sep values
}*/


public class SendAvatar : MonoBehaviour
{
    public Transform head;
    public Transform controllerRight;
    public Transform controllerLeft;
    //public Transform OBJECT;
    public Transform fingertipRight;
    public Transform fingertipLeft;

    /*public SaveTrailPoints saveTrailPoints;
    

    public bool sent = false;


    
    public string logPoints;*/
    public string logGeneral;

    public int port = 7001;
    private UdpBroadcast _upd;


    public bool SENDING = true; //TODO

    public Evaluation eval;

    void Start()
    {

    }

    public void Load()
    {
        _upd = new UdpBroadcast(port);
    }

    void Update()
    {
        if (!SENDING) return; // TODO 

        //sendCH = chlocal.sendCH;

        if (_upd != null)
        {
            /* Message is:
             * 
             *  head, controllerRight, controllerLeft, fingertipRight, fingertipLeft, pointingpoints
             * 
             */
            string msg = "";

            //msg += _getValues(OBJECT) + (char)MessageSeparators.L1;
            msg += _getValues(head) + (char)MessageSeparators.L1;
            msg += _getValues(controllerRight) + (char)MessageSeparators.L1;
            msg += _getValues(controllerLeft) + (char)MessageSeparators.L1;
            msg += _getValues(fingertipRight) + (char)MessageSeparators.L1;
            msg += _getValues(fingertipLeft);

            logGeneral = msg;

            /*if (saveTrailPoints.pointsTrail.Count != 0 && !saveTrailPoints.pressed && !sent && !eval.localIsDemonstrator)
            {
                
                logPoints = (char)MessageSeparators.L1 + _listToString(saveTrailPoints.pointsTrail);
                msg += logPoints;
                Debug.Log("Sent Once");
                sent = true;
            }*/


            _upd.send(msg);
            //Debug.Log(msg);
            
        }
    }

    private string _getValues(Transform t)
    {
        string ret = "";

        ret += _positionToString(t.position) + (char)MessageSeparators.L2;
        ret += _rotationToString(t.rotation);

        return ret;
    }

    private string _positionToString(Vector3 p)
    {
        return "" + p.x + (char)MessageSeparators.L3 + p.y + (char)MessageSeparators.L3 + p.z;

    }

    private string _rotationToString(Quaternion r)
    {
        return "" + r.x + (char)MessageSeparators.L3 + r.y + (char)MessageSeparators.L3 + r.z + (char)MessageSeparators.L3 + r.w;
    }

    /*private string _listToString(List<Vector3> points)
    {
        string msg = "";
        int i;

        //Debug.Log("Nr points sent: " + points.Count);

        for (i = 0; i < points.Count; i++)
        {
            msg += _positionToString(points[i]) + (char)MessageSeparators.L2;
        }

        //Debug.Log(msg);

        return msg;
    }*/

}
