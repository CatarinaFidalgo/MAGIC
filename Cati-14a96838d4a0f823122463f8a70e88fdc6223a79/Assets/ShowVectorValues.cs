using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVectorValues : MonoBehaviour
{
    public Transform avatarWrist;
    public Transform controllerWrist;

    public Vector3 avatarWristPosition;
    public Vector3 controllerWristPosition;

    // Update is called once per frame
    void Update()
    {
        avatarWristPosition = avatarWrist.position;
        controllerWristPosition = controllerWrist.position;

    }
}
