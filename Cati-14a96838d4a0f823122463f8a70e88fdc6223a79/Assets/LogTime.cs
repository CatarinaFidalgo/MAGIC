using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class LogTime : MonoBehaviour
{
    public Evaluation evaluation;
    public checkIntersection checkInt;

    private string participantName;
    private string t0;
    private string tf;
    private float delta_t;
    private string path;

    void Start()
    {
        t0 = System.DateTime.Now.ToString("HH:mm:ss");
        participantName = evaluation.participantID;

        //Path to the file
        path = Application.dataPath + "/" + participantName + "_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm").Replace(":",".").Replace("/", ".") + "_" + evaluation.condition + "_LogTime" + ".txt";
        //File.Create(path);
        
        /*
        if (evaluation.condition == ConditionType.Approach)
        {
            File.AppendAllText(path, "Condition: Approach\n");
        }

        else if (evaluation.condition == ConditionType.Veridical)
        {
            File.AppendAllText(path, "Condition: Veridical\n");
        }

        else
        {
            File.AppendAllText(path, "Condition: Side To Side\n");
        }
        */

        //When the file is created the beggining time of the experiment is written in the file

        File.AppendAllText(path, "Start Time: " + t0 + "\n");
    }

    void Update()
    {
        //if File exists we just add content to it

        if (!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying)
        {
            tf = System.DateTime.Now.ToString("HH:mm:ss");

            delta_t = StringToMinutes(tf) - StringToMinutes(t0);
            /*Debug.Log(t0);
            Debug.Log(tf);
            Debug.Log(delta_t);*/

            File.AppendAllText(path, "End Time: " + tf + "\n");

            /*if (checkInt.writeResult)
            {
                Debug.Log("Writing result of intersection on file");
                File.AppendAllText(path, "Volumes:\nLocal V: " + checkInt.chlocal.volume + " RemoteV: " + checkInt.chremote.volume + " Union V: " + checkInt.volumeOfUnion + " Intersection V: " + checkInt.volumeOfIntersection + ":   Percentage: " + checkInt.percentageOfIntersection + "%");

            }*/
            File.AppendAllText(path, "Volumes:\nLocal V: " + checkInt.chlocal.volume + " RemoteV: " + checkInt.chremote.volume + " Union V: " + checkInt.volumeOfUnion + " Intersection V: " + checkInt.volumeOfIntersection + ":   Percentage: " + checkInt.percentageOfIntersection + "%");
            File.AppendAllText(path, "Duration of the Experiment: " + MinutesToString(delta_t));
            //Debug.Log("Exiting playmode.");
        }

        /*else
        {
            Debug.Log("Correndo");
        }*/
    }

    float StringToMinutes(string timestring)
    {
        string[] parcels = timestring.Split(':');

        return (float.Parse(parcels[0]) * 60.0f + float.Parse(parcels[1]) + float.Parse(parcels[2]) / 60.0f);

    }


    string MinutesToString(float time)
    {
        double seconds = Math.Truncate((time - Math.Truncate(time)) * 60.0f);

        return "00:" + Math.Truncate(time) + ":" + seconds + "\n\n\n";

    }
}
