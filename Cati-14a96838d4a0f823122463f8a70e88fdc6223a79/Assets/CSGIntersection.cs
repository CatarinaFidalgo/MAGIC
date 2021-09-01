using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;
using System.Collections;
//using ConstructiveSolidGeometry;

public class MeshCSGOperation : MonoBehaviour
{

    /*
     *  Apply a CSG operation to the meshes for specified GameObjects a and b.
     *  If a and b are not specified (null), grab the meshes from the first to children of the transform.
     *  newObjectPrefab is cloned and given the resulting mesh after the CSG operation.
     */

    public enum Operation { Subtract, Union, Intersection };
    public Operation operation;
    public GameObject a;
    public GameObject b;
    public GameObject newObjectPrefab;

    void Start()
    {

        Transform[] children = new Transform[2];
        if (a == null && b == null)
        {
            int i = 0;
            foreach (Transform t in transform)
            {
                if (i > 2) break;
                children[i] = t;
                i++;
            }
        }
        else
        {
            children[0] = a.transform;
            children[1] = b.transform;
        }
        /*CSG A = CSG.fromMesh(children[0].GetComponent<MeshFilter>().mesh, children[0]);
        CSG B = CSG.fromMesh(children[1].GetComponent<MeshFilter>().mesh, children[1]);

        CSG result = null;        
        
        result = A.intersect(B);*/
      
        /*
         * Debug.Log(A.polygons.Count + ", " + B.polygons.Count + ", " + result.polygons.Count);
        foreach (Polygon p in result.polygons) {
            Debug.Log("Result: " + p.vertices[0].pos+", "+p.vertices[1].pos+", "+p.vertices[2].pos);
            if (p.vertices.Length > 3) Debug.Log("!!! " + p.vertices.Length);
        }
        */

        GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        //if (result != null) newGo.GetComponent<MeshFilter>().mesh = result.toMesh();
        children[0].gameObject.SetActive(false);
        children[1].gameObject.SetActive(false);
    }
}
