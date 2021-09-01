using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTarget : MonoBehaviour
{
    public Transform localTip;
    public Transform remoteWrist;
    public Transform remoteTip;

    private Vector3 wristToTip;
    private Vector3 tipToTip;
    //private float magnitude;

    //public Transform bolinha;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //tipToTip = localTip.position - remoteTip.position;
        // Debug.Log(tipToTip);

        //transform.position = remoteWrist.localPosition + tipToTip;
        //transform.rotation = remoteWrist.rotation;


        //transform.position = remoteWrist.localPosition;
        //transform.position = localTip.position; //faz com que o pulso remoto seja controlado pela ponta do dedo do localAvatar


        //wristToTip = remoteTip.position - remoteWrist.position;
        //magnitude = wristToTip.magnitude;

        //bolinha.position = remoteWrist.position + wristToTip;
        //bolinha.position = bolinha.position + new Vector3(0, 0, magnitude);

        






    }
    private void FixedUpdate()
    {
        //transform.position = remoteWrist.localPosition;
        
        //transform.rotation = remoteWrist.rotation;

        //tipToTip = localTip.position - remoteTip.position;
        //Debug.Log(tipToTip);
        //transform.position = remoteWrist.localPosition + tipToTip;

       //find the vector pointing from our position to the localTip (our target)
        //_direction = (localTip.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        //_lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 1);
    }
}
