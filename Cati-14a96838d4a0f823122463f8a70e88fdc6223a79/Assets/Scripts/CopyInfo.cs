using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyInfo : MonoBehaviour
{
    public Transform source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = source.position;
        transform.rotation = source.rotation;
    }
}
