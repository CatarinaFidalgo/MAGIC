using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class CSV_Generator : MonoBehaviour
{
    public Evaluation evaluation;
    public SendAvatar sendAvatar;

    private string participantName;
    private string path;



    void Start()
    {
        participantName = evaluation.participantID;
        //Path to the file
        //path = Application.dataPath + "/LogGeneral_" + participantName + "_" + t0 + "_" + evaluation.condition + ".txt";
        path = Application.dataPath + "/" + participantName + "_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm").Replace(":", ".").Replace("/", ".") + "_" + evaluation.condition + "_CSV" + ".txt";
        //File.Create(path);
    }


    void Update()
    {
        //System.DateTime.Now.ToString("HH:mm:ss");
        File.AppendAllText(path, "\"" + System.DateTime.Now.ToString("HH:mm:ss") + "\",\"" + sendAvatar.logGeneral.Replace("#", "\",\"").Replace(":", "\",\"").Replace("/", "\",\"") + "\"" + "\n");
    }
}
