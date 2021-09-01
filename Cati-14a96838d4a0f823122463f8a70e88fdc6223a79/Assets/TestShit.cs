using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShit : MonoBehaviour
{
    int i = 0;
    private Transform previous;

    void Start()
    {
        previous = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (i <= 300)
        {
            previous.GetComponent<Renderer>().material.SetColor("_Color", Color.HSVToRGB(148.0f / 360.0f, 1, 1));
            //Debug.Log("Green");
        }

        else
        {
            //transform.GetComponent<Material>().SetColor("_Color", Color.black);
            previous.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            //Debug.Log("Blaack");
        }

        i++;
        

    }
}
