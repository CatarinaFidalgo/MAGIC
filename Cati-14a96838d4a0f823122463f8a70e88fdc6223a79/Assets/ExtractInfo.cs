using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


//Saves the information of the controllers

public class ExtractInfo : MonoBehaviour
{
    public Evaluation evaluation;

    private string participantName;
    private string path;

    public Transform localControllerRight;
    public Transform localControllerLeft;
    public Transform localHMD;

    private int i;
    private string[] lines;

    /*public Transform remoteControllerRight;
    public Transform remoteControllerLeft;
    public Transform remoteHMD;*/


    void Start()
    {
        return;
        participantName = evaluation.participantID;

        //Path to the file
        path = Application.dataPath + "/LogGeneral_" + participantName + "_" + ConditionType.Approach + ".txt";


        // Read each line of the file into a string array. Each element of the array is one line of the file.
        //Text Order: LocalControllerRight Position | localControllerLeft Position | localHMD Position|localControllerRight Rotation | localControllerLeft Rotation | localHMD Rotation|[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");

        lines = File.ReadAllLines(@path);

        i = 0;
    }

    void Update()
    {
        return;
        if (i < lines.Length)
        {
            _parseString(lines[i]);
            Debug.Log(lines[i] + "\n");

        }

        i++;
    }

    private void _parseString(string s)
    {
        if (s != "")
        {
            string[] transforms = s.Split((char)MessageSeparators.L1);

            //OBJECT.localPosition = _parsePosition(transforms[0].Split((char)MessageSeparators.L2)[0]);
            //OBJECT.localRotation = _parseRotation(transforms[0].Split((char)MessageSeparators.L2)[1]);

            localHMD.localPosition = _parsePosition(transforms[0].Split((char)MessageSeparators.L2)[0]);
            localHMD.localRotation = _parseRotation(transforms[0].Split((char)MessageSeparators.L2)[1]);

            localControllerRight.localPosition = _parsePosition(transforms[1].Split((char)MessageSeparators.L2)[0]);
            localControllerRight.localRotation = _parseRotation(transforms[1].Split((char)MessageSeparators.L2)[1]);

            localControllerLeft.localPosition = _parsePosition(transforms[2].Split((char)MessageSeparators.L2)[0]);
            localControllerLeft.localRotation = _parseRotation(transforms[2].Split((char)MessageSeparators.L2)[1]);

            /*localFingertipRight.localPosition = _parsePosition(transforms[3].Split((char)MessageSeparators.L2)[0]);
            localFingertipRight.localRotation = _parseRotation(transforms[3].Split((char)MessageSeparators.L2)[1]);

            localFingertipLeft.localPosition = _parsePosition(transforms[4].Split((char)MessageSeparators.L2)[0]);
            localFingertipLeft.localRotation = _parseRotation(transforms[4].Split((char)MessageSeparators.L2)[1]);
            */

        }
    }

    private Quaternion _parseRotation(string v)
    {
        Quaternion ret = Quaternion.identity;

        string[] values = v.Split((char)MessageSeparators.L3);

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


}
