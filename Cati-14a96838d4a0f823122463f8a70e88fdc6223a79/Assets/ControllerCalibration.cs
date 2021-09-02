using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCalibration : MonoBehaviour
{
    public Evaluation evaluation;
    public OVRCameraRig cam;


    // Start is called before the first frame update
    void Start()
    {
        if (evaluation.machine == MachineType.A)
            cam.transform.position = new Vector3 (-0.76f , 0f, -0.55f);

        if (evaluation.machine == MachineType.B)
            cam.transform.position = new Vector3(0.13f, 0f, -0.37f);
    }

    
}
