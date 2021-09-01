using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructiveSolidGeometry;
using GK;
using Oculus.Platform.Samples.VrHoops;
using System.Linq;
using System;
public class ConvexHullIntersection : MonoBehaviour
{

    public GameObject a;
    public GameObject b;
    public List<Vector3> intersectionPoints = new List<Vector3>();
    public float volume;
    public Transform ball;
    public GameObject newObjectPrefab;
    public List<Vector3> ballsList = new List<Vector3>();
    public Transform POINTS;
    public bool readyRemote = false;
    public bool readylocal = false;

    //public IntersectionCSGTry intersectionCSG;
    void Start()
    {
        //Intersection through CSG

        if (readyRemote && readylocal)
        {
            CSG A = CSG.fromMesh(a.GetComponent<MeshFilter>().mesh, a.transform.GetChild(0));
            CSG B = CSG.fromMesh(b.GetComponent<MeshFilter>().mesh, b.transform.GetChild(0));

            Debug.Log("Antes Interseção");

            CSG result = A.intersect(B);

            //Debug.Log("Depois Interseção");

            intersectionPoints = CSGtoListOfPoints(result.polygons);

            //Debug.Log("Depois dos pontos: " + intersectionPoints.Count + "pontos");

            //Parametros necessarios para o algoritmo de Convex Hull

            var calc = new ConvexHullCalculator();
            var verts = new List<Vector3>();
            var tris = new List<int>();
            var normals = new List<Vector3>();

            //Debug.Log("Antes da CH" );

            for (int pi = 0; pi < intersectionPoints.Count; pi++)
            {
                Instantiate(ball, intersectionPoints[pi], Quaternion.identity, POINTS);
                //ballsList.Add((Instantiate(ball, intersectionPoints[pi], Quaternion.identity)).position);
                //Debug.Log("Tau");

            }

            for (int pi = 0; pi < POINTS.childCount; pi++)
            {
                ballsList.Add(POINTS.GetChild(pi).position);

            }



            calc.GenerateHull(ballsList, true, ref verts, ref tris, ref normals);

            //Debug.Log("Depois da CH");

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

            //intersectionCSG.intersectionPoints.Clear();

            newObjectPrefab.SetActive(false);
            a.SetActive(false);
            b.SetActive(false);
        }


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
        //List<Vector3> points = new List<Vector3>();
        HashSet<Vector3> hash = new HashSet<Vector3>();

        int i = 0;

        for (int pi = 0; pi < polygons.Count; pi++)
        {
            for (int v = 0; v < polygons[pi].vertices.Length; v++)
            {
                hash.Add(polygons[pi].vertices[v].pos);
                //points.Add(Instantiate(ball, points[i], Quaternion.identity).position);
                //Debug.Log(points[i].ToString("F7")); //point.ToString("F4")


                /*if (points[i].ToString("F7") == new Vector3(-0.6149451f, 0.3548579f, -0.2110606f).ToString("F7"))
                {
                    Debug.Log("Encontrei o sacana no i: " + i);
                    Debug.Log("Sacana coords: " + points[i].ToString("F15"));
                    
                } */

                i++;
            }

        }

        //Ponto contido no cubo inteiro: (-0.6149451f, 1.354858f, -0.2110606f)

        //points.Add(new Vector3(-0.6149451f, 0.3548579f, -0.2110606f)); //Para o inteiro


        //points.Add(new Vector3(-0.114945f, 0.3548579f, -0.2110606f)); //Para o meio
        //points.Add(new Vector3(-0.114945f, 1.354858f, -0.2110606f)); //Para o meio

        //points.Add(new Vector3(-0.5f, -0.5f, 0.5f)); //Para o inteiro

        //hash.Add(new Vector3(-0.5f, -0.5f, 0.5f));

        Debug.Log("Number of points in the array:" + hash.Count);
        Debug.Log("Number of i:" + i);

        List<Vector3> points = hash.ToList();

        return points;
    }
}
