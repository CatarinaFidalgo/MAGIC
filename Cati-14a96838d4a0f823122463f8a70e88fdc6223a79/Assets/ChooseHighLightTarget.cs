using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class ChooseHighLightTarget : MonoBehaviour
{
    //public int i = 1;
    public int j = 0;

    public List<Vector3> Test1 = new List<Vector3>();
    public List<Vector3> Test2 = new List<Vector3>();
    public List<Vector3> Test3 = new List<Vector3>();
    public List<Vector3> Test4 = new List<Vector3>();
    public List<Vector3> TestOn1 = new List<Vector3>();
    public List<Vector3> TestOn2 = new List<Vector3>();
    public List<Vector3> Test = new List<Vector3>();


    public Evaluation evaluation;
    public StartEndLogs startEnd;

    public Transform target;
    public Transform p101;
    public Transform p102;
    public Transform p103;

    public bool END = false;




    void Start()
    {
        FillTargetPoints(Test1, Test2, Test3, Test4);
        ChooseTargetTests();
    }

    
    void Update()
    {
        //Debug.Log(j);
        if (evaluation.machine == MachineType.A)
        {
            Test = TestOn1;
        }

        if (evaluation.machine == MachineType.B)
        {
            Test = TestOn2;
        }

        if (evaluation.localIsDemonstrator)
        {
            if (j < 5)
            {
                //101 active
                target.position = Test[j];

                p101.GetComponent<HighLight>().radius = 0.05f;
                p102.GetComponent<HighLight>().radius = 0.0f;
                p103.GetComponent<HighLight>().radius = 0.0f;

                
            }

            

            else if (j >= 5 && j < 12)
            {
                //102 active
                target.position = Test[j];

                p101.GetComponent<HighLight>().radius = 0.0f;
                p102.GetComponent<HighLight>().radius = 0.05f;
                p103.GetComponent<HighLight>().radius = 0.0f;
            }

            else if (j >= 12 && j < 16)
            {
                //103 active
                target.position = Test[j];

                p101.GetComponent<HighLight>().radius = 0.0f;
                p102.GetComponent<HighLight>().radius = 0.0f;
                p103.GetComponent<HighLight>().radius = 0.05f;
            } 

            if (j >= 16) 
            
            {
                 
                evaluation.localIsDemonstrator = !evaluation.localIsDemonstrator;
                //send message to other saying he is the demonstrator now

                if (evaluation.machine == MachineType.A)
                {
                    evaluation.tcpServer.SendAVeryImportantMessage("demonstrator#" + (!evaluation.localIsDemonstrator).ToString());
                    //Debug.Log("Sending in machine A");
                    
                }
                else
                {
                    END = true;
                    //evaluation.tcpClient.SendAVeryImportantMessage("demonstrator#" + (!evaluation.localIsDemonstrator).ToString());
                    p101.GetComponent<HighLight>().radius = 0.0f;
                    p102.GetComponent<HighLight>().radius = 0.0f;
                    p103.GetComponent<HighLight>().radius = 0.0f;
                }

                j = 0;

                //END = true;

            }

            /*if (j >= 16 && j < 16 + 5)
            {
                //101 active
                target.position = TestOn1[j - 16];

                p101.GetComponent<HighLight>().radius = 0.05f;
                p102.GetComponent<HighLight>().radius = 0.0f;
                p103.GetComponent<HighLight>().radius = 0.0f;

                
            }

            else if (j >= 16 + 5 && j < 16 + 12)
            {
                //102 active
                target.position = TestOn1[j - 16];

                p101.GetComponent<HighLight>().radius = 0.0f;
                p102.GetComponent<HighLight>().radius = 0.05f;
                p103.GetComponent<HighLight>().radius = 0.0f;
            }

            else if (j >= 16 + 12 && j < 16 + 16)
            {
                //103 active
                target.position = TestOn1[j - 16];

                p101.GetComponent<HighLight>().radius = 0.0f;
                p102.GetComponent<HighLight>().radius = 0.0f;
                p103.GetComponent<HighLight>().radius = 0.05f;
            }

            else
            {
                END = true;

            }*/
        }
        

    }

    void FillTargetPoints(List<Vector3> Test1, List<Vector3> Test2, List<Vector3> Test3, List<Vector3> Test4)
    {

        

        

        string[] lines = System.IO.File.ReadAllLines(Application.dataPath + "/TargetPoints.txt");
   
        //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\VIMMI3D\Documents\GitHub\Cati\Assets\TargetPointsFAST.txt");

        //int i = 0;

        //Debug.Log("Nr Lines in Text File: " + lines.Length);

        foreach (string line in lines)
        {
            string[] c = line.Split('#');

            //Debug.Log("Nr entries in line: " + c.Length);

           

            Test1.Add(new Vector3(float.Parse(c[0]), float.Parse(c[1]), float.Parse(c[2])));
            Test2.Add(new Vector3(float.Parse(c[3]), float.Parse(c[4]), float.Parse(c[5])));
            Test3.Add(new Vector3(float.Parse(c[6]), float.Parse(c[7]), float.Parse(c[8])));
            Test4.Add(new Vector3(float.Parse(c[9]), float.Parse(c[10]), float.Parse(c[11])));
            //i++;
            //Debug.Log(i);
        }
        
        return;
    }

    void ChooseTargetTests()
    {
        //Choose the first test
        if (evaluation.test1 == TestType.T1)
        {
            TestOn1 = Test1;
        }
        else if (evaluation.test1 == TestType.T2)
        {
            TestOn1 = Test2;
        }
        else if (evaluation.test1 == TestType.T3)
        {
            TestOn1 = Test3;
        }
        else
        {
            TestOn1 = Test4;
        }

        //Choose the second test
        if (evaluation.test2 == TestType.T1)
        {
            TestOn2 = Test1;
        }
        else if (evaluation.test2 == TestType.T2)
        {
            TestOn2 = Test2;
        }
        else if (evaluation.test1 == TestType.T3)
        {
            TestOn2 = Test3;
        }
        else
        {
            TestOn2 = Test4;
        }

    }

}
