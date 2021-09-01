using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageSeparators
{
    L1 = '#', // sep transforms
    L2 = '/', // sep propertines 
    L3 = ':' // sep values
}


public class ListToString : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    string message;
    public List<Vector3> pointsAfter = new List<Vector3>();

    // Update is called once per frame
    void Start()
    {
        

        for (int i = 0; i < 10; i++)
        {
            points.Add(Random.insideUnitSphere);
        }

        message = _listToString(points);

        pointsAfter = _stringToList(message);

        Debug.Log("cm<sup>3</sup>");

    }

    //remoteControllerRight.localPosition = _parsePosition(transforms[1].Split((char)MessageSeparators.L2)[0]);

    private List <Vector3> _stringToList( string msg)
    {
        List<Vector3> points = new List<Vector3>();

        string[] stringPoints = msg.Split((char)MessageSeparators.L2);
        foreach (string sp in stringPoints)
        {
            points.Add(_parsePosition(sp));
        }
        
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

    private string _listToString(List<Vector3> points)
    {
        string msg = "";
        int i;

        for (i = 0; i < points.Count - 1; i++)
        {
            msg += _positionToString(points[i]) + (char)MessageSeparators.L2;
        }

        msg += _positionToString(points[i]);

        return msg;
    }

    private string _positionToString(Vector3 p)
    {
        return "" + p.x + (char)MessageSeparators.L3 + p.y + (char)MessageSeparators.L3 + p.z;

    }
}
