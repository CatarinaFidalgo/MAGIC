using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using GK;

public class ReceiveTrail : MonoBehaviour
{
   
    //Strings for Loging
   
    public string logPoints;

    
    public List<Vector3> remotePoints;
    public bool receptionComplete = false;
    //public bool tcpWriteFile = true;


    public Evaluation eval;
    public StartEndLogs startEnd;
    public SendSetUpInfo sendToInterpreter;

    void Start()
    {
        remotePoints = new List<Vector3>();
    }

    
    void Update()
    {
    }

    public void newTrailMessage(string trailMessage)
    {
        if (trailMessage != "" && trailMessage.Split('#')[0] == "points")
        {
            
            
            if (receptionComplete == false && eval.localIsDemonstrator)
            {
                Debug.Log("Received once");
                
                remotePoints = _stringToList(trailMessage).ToList(); 

                logPoints = trailMessage;
                startEnd.showWorkspace = false;
                //sendToInterpreter.send("update#" + startEnd.showWorkspace.ToString());

                if (eval.machine == MachineType.A)
                {
                    eval.tcpServer.SendAVeryImportantMessage("update#" + startEnd.showWorkspace.ToString());
                    

                }
                else
                {
                    eval.tcpClient.SendAVeryImportantMessage("update#" + startEnd.showWorkspace.ToString());

                }

                receptionComplete = true;

                //tcpWriteFile = true;
            }
        }
    }

   

    private HashSet<Vector3> _stringToList(string msg)
    {
        HashSet<Vector3> points = new HashSet<Vector3>();

        string newMsg = msg.Split('#')[1];

        string[] stringPoints = newMsg.Split((char)MessageSeparators.L2);
        //Debug.Log(stringPoints.Length);
        //Debug.Log("a1");

        for (int i = 0; i < stringPoints.Length - 1; i++)
        {
            points.Add(_parsePosition(stringPoints[i]));
            //Debug.Log(points.ToList()[i]);
            //Debug.Log("points count: " + points.Count);

        }

        //Debug.Log("a3");
        return points;
    }

    private Vector3 _parsePosition(string v)
    {
        Vector3 ret = Vector3.zero;

        string[] values = v.Split((char)MessageSeparators.L3);

        ret.x = float.Parse(values[0]);
        ret.y = float.Parse(values[1]);
        ret.z = float.Parse(values[2]);

        return ret;
    }

}