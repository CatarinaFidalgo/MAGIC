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


public class SendTrail : MonoBehaviour
{
    

    public SaveTrailPoints saveTrailPoints;
    

    public bool sent = false;


    public string logPoints;

    public Evaluation eval;
    public StartEndLogs startEnd;

    //public bool erase = false;


    void Start()
    {

    }


    private void Update()
    {

        
        _updateLogPoints();

        if (logPoints.Length != 0)
        {
            if (eval.machine == MachineType.A)
            {
                eval.tcpServer.SendAVeryImportantMessage(logPoints);
                Debug.Log("Sending in machine A");
                logPoints = "";
                saveTrailPoints.pointsTrail.Clear();
                startEnd.showWorkspace = false;
            }
            else
            {
                eval.tcpClient.SendAVeryImportantMessage(logPoints);
                Debug.Log("Sending in machine B");
                logPoints = "";
                saveTrailPoints.pointsTrail.Clear();
                startEnd.showWorkspace = false;
            }

        }

        
    }

    private void _updateLogPoints()
    {
        if (saveTrailPoints.pointsTrail.Count != 0 && !saveTrailPoints.pressed && !sent && !eval.localIsDemonstrator)
        {
            logPoints = _listToString(saveTrailPoints.pointsTrail);
            Debug.Log("Sent Once " + logPoints.Length);
            sent = true;
        }

    }
    
    private string _listToString(List<Vector3> points)
    {
        string msg = "";
        int i;

        //Debug.Log("Nr points sent: " + points.Count);

        for (i = 0; i < points.Count; i++)
        {
            msg += _positionToString(points[i]) + (char)MessageSeparators.L2;
        }

        //Debug.Log(msg);

        return "points#" + msg;
    }

    private string _positionToString(Vector3 p)
    {
        return "" + _round(p.x) + (char)MessageSeparators.L3 + _round(p.y) + (char)MessageSeparators.L3 + _round(p.z);

    }

    private string _round(float f)
    {
        return "" + Math.Round(f, 2);
    }

}