using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GK;

public class StartEndLogs : MonoBehaviour
{
    //public string _resultsFolder;
    //public Role role;
    
    public DateTime taskStartTime;
    public DateTime taskEndTime;

    public Evaluation evaluation;
    //public SendTrail tcpSend;    
    public ReceiveTrail tcpListener;
    public UdpListener udpListener;
    public SendAvatar udpSend;
    public IntersectionCalculator intersection;
    public SaveTrailPoints saveTrailPoints;
    public ConvexHullTry chlocal;
    public ConvexHullTryRemote chremote;
    public ChooseHighLightTarget test;

    private string participantID;
    private string _logBodyLocalPath;
    private string _logBodyRemotePath;
    private string _logVolumePointsPath;
    private string _logIntersectionPath;
    private string _logTimePath;

    private string generalInfo = "";

    
    public bool getStartTime = true;
    private bool getEndTime = true;
    public bool showWorkspace = false;

    public GameObject endCanvas;
    public GameObject startCanvas;
    public GameObject midCanvas;

    public SendSetUpInfo sendToInterpreter;

    //public bool changeCoordinator = true;
    public bool changeCoordinator = false;
    public bool filesCreated = false;

    //IEnumerator Start()
    void Start()
    {


        


           

    }

    
    void Update()
    {
        OVRInput.Update();

        //Debug.Log(changeCoordinator);

        ///Init files
        if (evaluation.localIsDemonstrator && test.j == 0 && changeCoordinator)
        {
            Debug.Log("InitializeFiles files");
            startCanvas.SetActive(true);
            midCanvas.SetActive(false);
            endCanvas.SetActive(false);            
            generalInfo = "Condition\",\"ParticipantID\",\"Test\",\" ";
            Debug.Log(generalInfo);
            InitializeFiles();
            changeCoordinator = false;
        }

        string test1 = "";

        if (evaluation.machine == MachineType.A)
        {
            test1 = evaluation.test1.ToString();
        }

        else if (evaluation.machine == MachineType.B)
        {
            test1 = evaluation.test2.ToString();
        }

        
        

        if (evaluation.localIsDemonstrator && !test.END && filesCreated)
        {
            /*if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.9f) //&& getStartTime)
            {
                Debug.Log("Secondary");
            }

            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.9f) //&& getStartTime)
            {
                Debug.Log("Primary");
            }

            Debug.Log("Right: " + OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));
            Debug.Log("Left: " + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger));
            //Debug.Log("Rest: " + OVRInput.Get(OVRInput.Touch.SecondaryThumbRest));
            Debug.Log("B: " + OVRInput.Get(OVRInput.Button.Two));
            Debug.Log("A: " + OVRInput.Get(OVRInput.Button.One));*/



            //if (trail.pressed && getStartTime) //CHANGE THE START METHOD, PRESS ANOTHER BUTTON

            if(((OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.9f) || (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.9f)) && getStartTime)
            {
                showWorkspace = true;

                //sendToInterpreter.send("update#" + showWorkspace.ToString());

                if (evaluation.machine == MachineType.A)
                {
                    evaluation.tcpServer.SendAVeryImportantMessage("update#" + showWorkspace.ToString());
                    //Debug.Log("Sending in machine A");

                }
                else
                {
                    evaluation.tcpClient.SendAVeryImportantMessage("update#" + showWorkspace.ToString());

                }

                midCanvas.SetActive(false);
                startCanvas.SetActive(false);

                taskStartTime = DateTime.Now;
                getStartTime = false;

                Debug.Log("Started outline");
            }

            if (tcpListener.receptionComplete && getEndTime)
            {
                
                taskEndTime = DateTime.Now;
                getEndTime = false;

                //showWorkspace = false;

                //sendToInterpreter.send("update#" + showWorkspace.ToString());

                midCanvas.SetActive(true);

                test.j++; //Change target
                //Debug.Log("j: " + test.j);

               

                Debug.Log("Outline completed by partner");
                



                //Debug.Log("End Time:" + taskEndTime.ToString());
                File.AppendAllText(_logTimePath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + taskStartTime.ToString("HH:mm:ss:fff") + "\",\"" + taskEndTime.ToString("HH:mm:ss:fff") + "\",\"" + DeltaDateTimeToSeconds(taskStartTime, taskEndTime).ToString() + "\"\n");

            }

            if (chlocal.writeFile)
            {
                //Save drawn data points (Local)
                string logPoints = _listToString(saveTrailPoints.pointsTrail);

                File.AppendAllText(_logVolumePointsPath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + System.DateTime.Now.ToString("HH:mm:ss:fff") + logPoints.Replace(",",".").Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\"\n");
                chlocal.writeFile = false;
                Debug.Log("TrailPointsLocal");

            }

            if (chremote.writeFile)
            {
                //Save drawn data points (Remote)
                File.AppendAllText(_logVolumePointsPath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + System.DateTime.Now.ToString("HH:mm:ss:fff") + "\",\"" + tcpListener.logPoints.Replace(",", ".").Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\"\n");
                chremote.writeFile = false;
                Debug.Log("TrailPointsRemote");

            }

            if (intersection.writeResult)
            {                            
                //Save volumes and intersection
                File.AppendAllText(_logIntersectionPath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + System.DateTime.Now.ToString("HH:mm:ss:fff") + "\",\"" + intersection.percentageString.Replace(",", ".").Replace("#", "\",\"") + "\"\n");
                

                //Update booleans

                getStartTime = true;
                getEndTime = true;                
                intersection.writeResult = false;

                Debug.Log("Intersection completed");
                

            }

            //Save body data for both participants
            File.AppendAllText(_logBodyLocalPath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + System.DateTime.Now.ToString("HH:mm:ss:fff") + "\",\"" + udpSend.logGeneral.Replace(",", ".").Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\"" + "\n");
            File.AppendAllText(_logBodyRemotePath,  evaluation.condition.ToString() + "\",\"" + evaluation.participantID.ToString() + "\",\"" + test1 + "\",\"" + System.DateTime.Now.ToString("HH:mm:ss:fff") + "\",\"" + udpListener.logGeneral.Replace(",", ".").Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\"" + "\n");

            //+ "\",\"" + sendAvatar.logGeneral.Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\""


        }        
        

        else if (test.END)
        {
            endCanvas.SetActive(true);
            midCanvas.SetActive(false);
            //Debug.Log("Está no else");
        }

        else
        {
            endCanvas.SetActive(false);
            midCanvas.SetActive(false);
        }




        //if local is not the demonstrator the data won't be stored in his unit
        //Logs are always saved in the demonstrators site


        //yield return new WaitForSeconds(0.0f);
    }

    void InitializeFiles()
    {
        participantID = evaluation.participantID;

        evaluation._resultsFolder = Application.dataPath + Path.DirectorySeparatorChar + evaluation._resultsFolder;


        if (!Directory.Exists(evaluation._resultsFolder))
        {
            Directory.CreateDirectory(evaluation._resultsFolder);
            Debug.Log("Folder created: " + evaluation._resultsFolder);
        }
        else
        {
            print("Folder already exists: " + evaluation._resultsFolder);
        }
        
        //Create files

        _logBodyLocalPath = evaluation._resultsFolder + "/" + participantID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" +  "_LogBodyLocal" + ".txt";
        _logBodyRemotePath = evaluation._resultsFolder + "/" + participantID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" +  "_LogBodyRemote" + ".txt";
        _logVolumePointsPath = evaluation._resultsFolder + "/" + participantID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" +  "_LogVolumePoints" + ".txt";
        _logIntersectionPath = evaluation._resultsFolder + "/" + participantID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" +  "_LogIntersection" + ".txt";
        _logTimePath = evaluation._resultsFolder + "/" + participantID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_" +  "_LogTime" + ".txt";

        //Fill 1st line with description
        File.AppendAllText(_logBodyLocalPath, generalInfo + "\"Time Stamp\",\"Local_HMD_p_x\",\"Local_HMD_p_y\",\"Local_HMD_p_z\",\"Local_HMD_r_x\",\"Local_HMD_r_y\",\"Local_HMD_r_z\",\"Local_HMD_r_w\",\"Local_Right_p_x\",\"Local_Right_p_y\",\"Local_Right_p_z\",\"Local_Right_r_x\",\"Local_Right_r_y\",\"Local_Right_r_z\",\"Local_Right_r_w\",\"Local_Left_p_x\",\"Local_Left_p_y\",\"Local_Left_p_z\",\"Local_Left_r_x\",\"Local_Left_r_y\",\"Local_Left_r_z\",\"Local_Left_r_w\",\"Local_FingertipR_p_x\",\"Local_FingertipR_p_y\",\"Local_FingertipR_p_z\",\"Local_FingertipR_r_x\",\"Local_FingertipR_r_y\",\"Local_FingertipR_r_z\",\"Local_FingertipR_r_w\",\"Local_FingertipL_p_x\",\"Local_FingertipL_p_y\",\"Local_FingertipL_p_z\",\"Local_FingertipL_r_x\",\"Local_FingertipL_r_y\",\"Local_FingertipL_r_z\",\"Local_FingertipL_r_w\",\"\n");
        File.AppendAllText(_logBodyRemotePath, generalInfo + "\"Time Stamp\",\"Remote_HMD_p_x\",\"Remote_HMD_p_y\",\"Remote_HMD_p_z\",\"Remote_HMD_r_x\",\"Remote_HMD_r_y\",\"Remote_HMD_r_z\",\"Remote_HMD_r_w\",\"Remote_Right_p_x\",\"Remote_Right_p_y\",\"Remote_Right_p_z\",\"Remote_Right_r_x\",\"Remote_Right_r_y\",\"Remote_Right_r_z\",\"Remote_Right_r_w\",\"Remote_Left_p_x\",\"Remote_Left_p_y\",\"Remote_Left_p_z\",\"Remote_Left_r_x\",\"Remote_Left_r_y\",\"Remote_Left_r_z\",\"Remote_Left_r_w\",\"Remote_FingertipR_p_x\",\"Remote_FingertipR_p_y\",\"Remote_FingertipR_p_z\",\"Remote_FingertipR_r_x\",\"Remote_FingertipR_r_y\",\"Remote_FingertipR_r_z\",\"Remote_FingertipR_r_w\",\"Remote_FingertipL_p_x\",\"Remote_FingertipL_p_y\",\"Remote_FingertipL_p_z\",\"Remote_FingertipL_r_x\",\"Remote_FingertipL_r_y\",\"Remote_FingertipL_r_z\",\"Remote_FingertipL_r_w\",\"" + Environment.NewLine);
        File.AppendAllText(_logVolumePointsPath, generalInfo + "\"Set of points participants used to outline the area of interest (for replicability): Demonstrator then Interpreter \"\n");
        File.AppendAllText(_logIntersectionPath, generalInfo + "\"Time Stamp\",\"V_Local(cm3)\",\"V_Remote(cm3)\",\"V_Union(cm3)\",\"V_Intersection(cm3)\",\"Intersection (%)\"\n");
        File.AppendAllText(_logTimePath, generalInfo + "\"Task Start Time\",\"Task End Time\",\"Delta Time\"\n");

        filesCreated = true;
    }

    TimeSpan DeltaDateTimeToSeconds(DateTime t0, DateTime t1)
    {
        return t1 - t0;

        /**
        string[] parcels0 = t0.ToString("HH:mm:ss:fff").Split(':');
        string[] parcels1 = t1.ToString("HH:mm:ss:fff").Split(':');

        float sec0 = float.Parse(parcels0[0]) * 3600.0f + float.Parse(parcels0[1]) * 60.0f + float.Parse(parcels0[2]) + float.Parse(parcels0[3]) / 60.0f;
        float sec1 = float.Parse(parcels1[0]) * 3600.0f + float.Parse(parcels1[1]) * 60.0f + float.Parse(parcels1[2]) + float.Parse(parcels1[3]) / 60.0f;
        //Debug.Log(sec0 +"   "+ sec1 + "   " + (sec1 - sec0));
        return (sec1-sec0);
        **/
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

        return msg;
    }

    private string _positionToString(Vector3 p)
    {
        return "" + p.x + (char)MessageSeparators.L3 + p.y + (char)MessageSeparators.L3 + p.z;

    }


    /*string SecondsToString(float time)
    {
        double milisec = Math.Truncate((time - Math.Truncate(time)) * 60.0f);
        Debug.Log("MinutesToString");

        return "" + Math.Truncate(time) + ":" + milisec.ToString("F0");

    }*/
}
