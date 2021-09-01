using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTry : MonoBehaviour
{
    public Transform source;
    public Transform avatar;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = source.position;
        transform.position = new Vector3(- source.position.x, source.position.y, source.position.z);
        transform.rotation = source.rotation;


        //transform.RotateAround( avatar.position , Vector3.up, 90f);
    }
}
