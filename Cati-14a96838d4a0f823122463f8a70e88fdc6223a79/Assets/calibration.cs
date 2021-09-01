﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calibration : MonoBehaviour
{
    public Transform Avatar;
    public Transform AvatarAPAGA;

    public Transform leftShoulder;
    public Transform rightShoulder;

    public Transform localHandLeft;
    public Transform controllerLeft;

    public Transform localHandRight;
    public Transform controllerRight;

    public VRRig2 VRRig;

    private float _scale;
    public float Scale { get => _scale; }

    void Update()
    {
                    //return;

        if (Input.GetKeyDown(KeyCode.Space))
        {            
            float leftScale = Vector3.Distance(leftShoulder.position, controllerLeft.position) / Vector3.Distance(leftShoulder.position, localHandLeft.position);
            float rightScale = Vector3.Distance(rightShoulder.position, controllerRight.position) / Vector3.Distance(rightShoulder.position, localHandRight.position);

            _scale = (leftScale + rightScale) * 0.5f;

            Vector3 tmpPos = Avatar.position;
            Avatar.localScale = new Vector3(_scale, _scale, _scale);
            Avatar.position = tmpPos;

            VRRig.headBodyOffset *= _scale;

            tmpPos = AvatarAPAGA.position;
            AvatarAPAGA.localScale = new Vector3(_scale, _scale, _scale);
            AvatarAPAGA.position = tmpPos;

            Debug.Log("Scale: " + _scale);
        }
    }
}
