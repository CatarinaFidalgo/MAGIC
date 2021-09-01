using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using GK;

public class UdpListener : MonoBehaviour
{

    //public Transform OBJECT;
    public Transform remoteHead;
    public Transform remoteControllerRight;
    public Transform remotecontrollerLeft;
    public Transform remoteFingertipRight;
    public Transform remoteFingertipLeft;

    public string logGeneral;
    /*//Strings for Loging
    
    public string logPoints;

    //public ConvexHullTry chremote;
    public List<Vector3> remotePoints;
    public bool receptionComplete = false;
    public bool udpWriteFile = true;*/


    public int port;
    

    private UdpClient _udpClient = null;
    private IPEndPoint _anyIP;
    private List<byte[]> _stringsToParse; // TMA: Store the bytes from the socket instead of converting to strings. Saves time.
    private byte[] _receivedBytes;
    //so we don't have to create again

    void Start()
    {
        udpRestart();
        //remotePoints = new List<Vector3>();
    }

    public void udpRestart()
    {
        if (_udpClient != null)
        {
            _udpClient.Close();
        }

        _stringsToParse = new List<byte[]>();
        _anyIP = new IPEndPoint(IPAddress.Any, port);
        _udpClient = new UdpClient(_anyIP);
        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

    }

    public void ReceiveCallback(IAsyncResult ar)
    {
        Byte[] receiveBytes = _udpClient.EndReceive(ar, ref _anyIP);
        _udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
        _stringsToParse.Add(receiveBytes);
    }

    void Update()
    {
        while (_stringsToParse.Count > 0)
        {
            
            try
            {
                byte[] toProcess = _stringsToParse.First();
                
                if (toProcess != null)
                {
                    
                    string stringToParse = Encoding.ASCII.GetString(toProcess);
                    _parseString(stringToParse);

                }
                _stringsToParse.RemoveAt(0);
            }
            catch (Exception /*e*/) { _stringsToParse.RemoveAt(0); }
        }
    }

    private void _parseString(string s)
    {
        if (s != "")
        {
            string[] transforms = s.Split((char)MessageSeparators.L1);

            logGeneral = transforms[0] + (char)MessageSeparators.L1 + transforms[1] + (char)MessageSeparators.L1 + transforms[2] + (char)MessageSeparators.L1 + transforms[3] + (char)MessageSeparators.L1 + transforms[4];
            
            //OBJECT.localPosition = _parsePosition(transforms[0].Split((char)MessageSeparators.L2)[0]);
            //OBJECT.localRotation = _parseRotation(transforms[0].Split((char)MessageSeparators.L2)[1]);

            remoteHead.localPosition = _parsePosition(transforms[0].Split((char)MessageSeparators.L2)[0]);
            remoteHead.localRotation = _parseRotation(transforms[0].Split((char)MessageSeparators.L2)[1]);

            remoteControllerRight.localPosition = _parsePosition(transforms[1].Split((char)MessageSeparators.L2)[0]);
            remoteControllerRight.localRotation = _parseRotation(transforms[1].Split((char)MessageSeparators.L2)[1]);

            remotecontrollerLeft.localPosition = _parsePosition(transforms[2].Split((char)MessageSeparators.L2)[0]);
            remotecontrollerLeft.localRotation = _parseRotation(transforms[2].Split((char)MessageSeparators.L2)[1]);

            remoteFingertipRight.localPosition = _parsePosition(transforms[3].Split((char)MessageSeparators.L2)[0]);
            remoteFingertipRight.localRotation = _parseRotation(transforms[3].Split((char)MessageSeparators.L2)[1]);

            remoteFingertipLeft.localPosition = _parsePosition(transforms[4].Split((char)MessageSeparators.L2)[0]);
            remoteFingertipLeft.localRotation = _parseRotation(transforms[4].Split((char)MessageSeparators.L2)[1]);

            //Debug.Log("Length: " + transforms.Length);

            /*

            if (transforms.Length == 6 && receptionComplete == false)
            {
                Debug.Log("Received once");
                
                remotePoints = _stringToList(transforms[5]).ToList();
                Debug.Log(remotePoints.Count);
                logPoints = transforms[5];
                receptionComplete = true;
                udpWriteFile = true;
            }
            */
            

        }
    }

    private Quaternion _parseRotation(string v)
    {
        Quaternion ret = Quaternion.identity;

        string [] values = v.Split((char)MessageSeparators.L3);

        ret.x = float.Parse(values[0]);
        ret.y = float.Parse(values[1]); 
        ret.z = float.Parse(values[2]);
        ret.w = float.Parse(values[3]);

        return ret;
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

    /*private HashSet<Vector3> _stringToList(string msg)
    {
        HashSet<Vector3> points = new HashSet<Vector3>();

        string[] stringPoints = msg.Split((char)MessageSeparators.L2);
        //Debug.Log(stringPoints.Length);
        //Debug.Log("a1");

        for (int i = 0; i < stringPoints.Length - 1; i++)
        {
            points.Add(_parsePosition(stringPoints[i]));
            //Debug.Log(points.ToList()[i]);
            //Debug.Log(points.Count);

        }        

        //Debug.Log("a3");
        return points;
    }*/


    void OnApplicationQuit()
    {
        if (_udpClient != null) _udpClient.Close();
    }

    void OnQuit()
    {
        OnApplicationQuit();
    }
}