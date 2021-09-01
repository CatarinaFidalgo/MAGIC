using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFingertipValues : MonoBehaviour
{
    public Transform local;
    public Transform remote;
    public Transform warped;

    public Vector3 localP;
    public Vector3 localCalculatedP;
    public Vector3 remoteP;
    public Vector3 warpedP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        localP = local.position;
        localCalculatedP = new Vector3(-warped.localPosition.x, warped.localPosition.y, -warped.localPosition.z);
        remoteP = remote.localPosition;
        warpedP = warped.localPosition;
    }
}
