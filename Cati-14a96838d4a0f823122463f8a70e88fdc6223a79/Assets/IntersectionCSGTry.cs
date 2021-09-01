using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructiveSolidGeometry;
using GK;
using Oculus.Platform.Samples.VrHoops;
using System.Linq;
using System;

public class IntersectionCSGTry : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    //public GameObject newObjectPrefab;
    //public GameObject newGo;
    public List<Vector3> intersectionPoints = new List<Vector3>();
    //public float volume;
    //public Transform ball;
    //public List<Vector3> pointsToCH = new List<Vector3>(); 

    void Start()
    {
        //return;
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

        CSG A = CSG.fromMesh(children[0].GetComponent<MeshFilter>().mesh, children[0]);
        CSG B = CSG.fromMesh(children[1].GetComponent<MeshFilter>().mesh, children[1]);

        CSG result = null;
    
        result = A.intersect(B);

        

        intersectionPoints = CSGtoListOfPoints(result.polygons);

        /*for (int pi = 0; pi < intersectionPoints.Count; pi++)
        {
            float x = (float)Math.Ceiling(intersectionPoints[pi].x * 10000000) / 10000000;
            float y = (float)Math.Ceiling(intersectionPoints[pi].y * 10000000) / 10000000;
            float z = (float)Math.Ceiling(intersectionPoints[pi].z * 10000000) / 10000000;

            pointsToCH.Add(new Vector3(x, y, z));
        Instantiate(ball, intersectionPoints[pi], Quaternion.identity);

        }



        //Debug.Log("Number of points in the array after:" + pointsToCH.Count);

        //Parametros necessarios para o algoritmo de Convex Hull

        var calc = new ConvexHullCalculator();
        var verts = new List<Vector3>();
        var tris = new List<int>();
        var normals = new List<Vector3>();

        calc.GenerateHull(intersectionPoints, true, ref verts, ref tris, ref normals);

        //Create an initial transform that will evolve into our Convex Hull when altering the mesh

        var initialHull = Instantiate(newObjectPrefab);

        initialHull.transform.SetParent(transform, false);
        initialHull.transform.position = Vector3.zero;
        initialHull.transform.rotation = Quaternion.identity;
        initialHull.transform.localScale = Vector3.one;

        //Independentemente do tipo de mesh com que se começa (cubo, esfera..) 
        //a mesh é redefenida com as definiçoes abaixo

        var mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);

        initialHull.GetComponent<MeshFilter>().sharedMesh = mesh;
        initialHull.GetComponent<MeshCollider>().sharedMesh = mesh;

        //Calcular o volume da CH

        volume = VolumeOfMesh(mesh) * 1000000; //convert to cm3
        Debug.Log("Volume: " + volume);

        //Limpar os pontos antigos da lista para o proximo convex hull e
        //informar o programa de que já realizou esta função 

        //intersectionPoints.Clear();*/


        /*GameObject newGo = Instantiate(newObjectPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (result != null) newGo.GetComponent<MeshFilter>().mesh = result.toMesh();*/

        children[0].gameObject.SetActive(false);
        children[1].gameObject.SetActive(false);
        //newObjectPrefab.SetActive(false);


        //Debug.Log("Volume A: " + VolumeOfMesh(children[0].gameObject.GetComponent<MeshFilter>().mesh) * 1000000);

        //Debug.Log("Result Volume: " + VolumeOfMesh(newGo.GetComponent<MeshFilter>().mesh)*1000000);

        //Debug.Log("A verts, normals, tris: " + children[0].gameObject.GetComponent<MeshFilter>().mesh.vertices.Length + ", " + children[0].gameObject.GetComponent<MeshFilter>().mesh.normals.Length + ", " + children[0].gameObject.GetComponent<MeshFilter>().mesh.triangles.Length);
        Debug.Log("THIS before:" + intersectionPoints.Count);
    }

    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }

    List<Vector3> CSGtoListOfPoints(List<Polygon> polygons)
    {
        List<Vector3> points = new List<Vector3>();
        List<Vector3> pointsReturn = new List<Vector3>();

        int i = 0;

        Vector3 compare = new Vector3();

        //points.Add(new Vector3(-0.6149451f, 0.3548579f, -0.2110606f));

        for (int pi = 0; pi < polygons.Count; pi++)
        {
            for (int v = 0; v < polygons[pi].vertices.Length; v++) 
            {
                points.Add(polygons[pi].vertices[v].pos);
                
                //points.Add(Instantiate(ball, points[i], Quaternion.identity).position);
                Debug.Log(points[i].ToString("F7")); //point.ToString("F4")


                if(points[i].ToString("F7") == new Vector3(-0.6149451f, 0.3548579f, -0.2110606f).ToString("F7"))
                {
                    Debug.Log("Encontrei o sacana no i: " + i);
                    Debug.Log("Sacana coords: " + points[i].ToString("F15"));
                    compare = points[i];
                }

                if (points.Contains(compare))
                {
                    Debug.Log("Still here");
                }
                else
                {
                    Debug.Log("Not");
                }

                
                i++;

            }            
               
        }

        //Ponto contido no cubo inteiro: (-0.6149451f, 1.354858f, -0.2110606f)

        //points.Add(new Vector3(-0.6149451f, 0.3548579f, -0.2110606f)); //Para o inteiro
        //Debug.Log(points.Contains(new Vector3(-0.6149451f, 0.3548579f, -0.2110606f)));
        

        //points.Add(new Vector3(-0.114945f, 0.3548579f, -0.2110606f)); //Para o meio
        //points.Add(new Vector3(-0.114945f, 1.354858f, -0.2110606f)); //Para o meio


        Debug.Log("Number of points in the array:" + points.Count);
        Debug.Log("Number of i:" + i);
       

        

        return points;
    }


}
