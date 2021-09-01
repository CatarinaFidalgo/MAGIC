using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class CSV : MonoBehaviour
{

    public Evaluation evaluation;

    private string participantName;
    private string pathFrom;
    private string pathTo;
    private string[] lines;
    private string[] new_lines;
    private int i;
    private int count;
    private string t0;


    // Use this for initialization
    void Start()
    {
        return;
        t0 = System.DateTime.Now.ToString("HH:mm:ss");
        participantName = evaluation.participantID;


        //Path to the file
        //pathFrom = Application.dataPath + "/LogGeneral_" + participantName + "_" + ConditionType.Approach + ".txt";
        pathTo = Application.dataPath + "/CSV_" + participantName + "_" + t0 + "_" + evaluation.condition + ".txt";

        lines = File.ReadAllLines(@pathFrom);
        new_lines = lines;
        count = lines.Length;

        //File.AppendAllText(path, System.DateTime.Now.ToString("HH:mm:ss") + "#" + sendAvatar.logGeneral + "\n");

        for (i = 0; i < count; i++) //replace :
        {
            new_lines[i] = lines[i].Replace(":", "\",\"");
        }

        for (i = 0; i < count; i++) //replace #
        {
            lines[i] = new_lines[i].Replace("#", "\",\"");
        }

        for (i = 0; i < count; i++) //replace / and add the " in the beggining and end
        {
            new_lines[i] = "\"" + lines[i].Replace("/", "\",\"") + "\"";

            File.AppendAllText(pathTo, new_lines[i] + "\n");


        }

    }


}
