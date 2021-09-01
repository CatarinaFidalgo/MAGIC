using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructiveSolidGeometry;
using GK;
using Oculus.Platform.Samples.VrHoops;
using System.Linq;
using System;

[System.Serializable]
public class IntersectionCalculator : MonoBehaviour
{
    public ConvexHullTryRemote chremote;
    public ConvexHullTry chlocal;

    public GameObject localVolumesParent;
    public GameObject remoteVolumesParent;
    public GameObject IntParent;
    //public List<Vector3> intersectionPoints = new List<Vector3>();
    //public float volumeOfUnion;
    public float volumeOfIntersection;
    //public Transform ball;
    public GameObject initialMeshCSG;
    public GameObject meshLess;
    //public List<Vector3> ballsList = new List<Vector3>();
    //public Transform POINTS;
    public int child;
    public float percentageOfIntersection;
    //public bool intersectionDone = false;
    //public HashSet<Vector3> unionHash = new HashSet<Vector3>();
    //public List<Vector3> unionList = new List<Vector3>();
    public bool writeResult;
    public string percentageString;

    public Evaluation eval;
    public StartEndLogs startEnd;

    private Mesh intersectionMesh;
    public GameObject ball;


    void Update()
    {
        //writeResult = false;

        if (chremote.nrCHremote == chlocal.nrCHlocal && chremote.nrCHremote != 0 && chlocal.nrCHlocal != 0 && chlocal.readyForIntersectionLocal && chremote.readyForIntersectionRemote && eval.localIsDemonstrator)
        {
            //child = chremote.nrCHremote - 1;
            Debug.Log("Initiate Intersection");
            try
            {

                child = chremote.nrCHremote - 1;

                CSG A = CSG.fromMesh(localVolumesParent.transform.GetChild(child).GetComponent<MeshFilter>().mesh, localVolumesParent.transform.GetChild(child));
                CSG B = CSG.fromMesh(remoteVolumesParent.transform.GetChild(child).GetComponent<MeshFilter>().mesh, remoteVolumesParent.transform.GetChild(child));

                CSG result = null;
                result = A.intersect(B);

                Debug.Log(A.polygons.Count + ", " + B.polygons.Count + ", " + result.polygons.Count);

                GameObject newGo = Instantiate(meshLess, Vector3.zero, Quaternion.identity) as GameObject;
                newGo.transform.SetParent(IntParent.transform, false);
                intersectionMesh = result.toMesh();
                newGo.GetComponent<MeshFilter>().mesh = intersectionMesh;
                float volumeCSG = (VolumeOfMesh(intersectionMesh) * 1000000);
                Debug.Log("Volume from CSG Algorithm: " + volumeCSG + ".   %: " + volumeCSG / chlocal.volume * 100.0f);

                newGo.SetActive(false);
                


                ////////////////////////////////////////////////////////////////////////////////////
                ////////   PASS TO CONEX HULL /////////////////////

                //Parametros necessarios para o algoritmo de Convex Hull

                var calc = new ConvexHullCalculator();
                var verts = new List<Vector3>();
                var tris = new List<int>();
                var normals = new List<Vector3>();

                
                //Debug.Log("Intersection points Hash:" + result.intersectionPoints.Count);

                /*for (int i = 0; i < result.intersectionPoints.Count - 1; i++)
				{

					float x = (float)Math.Ceiling(result.intersectionPoints.ToList()[i].x * 10000000) / 10000000;
					float y = (float)Math.Ceiling(result.intersectionPoints.ToList()[i].y * 10000000) / 10000000;
					float z = (float)Math.Ceiling(result.intersectionPoints.ToList()[i].z * 10000000) / 10000000;

					Vector3 position = new UnityEngine.Vector3(x , y, z);

					Instantiate(ball, position, Quaternion.identity);

				}*/

                if (result.intersectionPoints.Count >= 4)
                {
                    calc.GenerateHull(result.intersectionPoints.ToList(), true, ref verts, ref tris, ref normals);

                    //Create an initial transform that will evolve into our Convex Hull when altering the mesh

                    var initialHull = Instantiate(initialMeshCSG);
                    //initialHull = Instantiate(initialMesh);

                    initialHull.transform.SetParent(IntParent.transform, false);
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
                    /////////////////////////////////////////////////////////////////////
                    
                    //Calcular o volume da mesh
                    volumeOfIntersection = VolumeOfMesh(mesh) * 1000000; //convert to cm3  

                    //Compare the volume of intersection with the volume that the local pointed

                    percentageOfIntersection = volumeOfIntersection / chlocal.volume * 100.0f;
                }



                else
                {
                    volumeOfIntersection = 0.0f;
                    percentageOfIntersection = 0.0f;
                }

                Debug.Log("Volume from CH Algorithm: " + volumeOfIntersection + ".   %: " + percentageOfIntersection);
                //Debug.Log("Percentage of Intersection: " + percentageOfIntersection.ToString("F0") + "%");

                percentageString = chlocal.volume.ToString() + '#' + chremote.volume.ToString() + '#' + volumeOfIntersection.ToString() + '#' + percentageOfIntersection.ToString("F1");
                writeResult = true;

                Debug.Log(percentageString);

                initialMeshCSG.SetActive(false);                
                localVolumesParent.transform.GetChild(child).gameObject.SetActive(false);
                remoteVolumesParent.transform.GetChild(child).gameObject.SetActive(false);



                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                Debug.Log("Intersection write = " + writeResult);

            }

            catch (System.ArgumentException)
            {


                initialMeshCSG.SetActive(false);
                localVolumesParent.transform.GetChild(child).gameObject.SetActive(false);
                remoteVolumesParent.transform.GetChild(child).gameObject.SetActive(false);


                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                /*foreach (Transform child in POINTS)
                {
                    Destroy(child.gameObject);
                }*/

                startEnd.getStartTime = true;
                percentageString = "Error computing: Can't generate hull, points are coplanar!";
                writeResult = true;

            }

            catch (UnityEngine.Assertions.AssertionException)
            {
                initialMeshCSG.SetActive(false);
                localVolumesParent.transform.GetChild(child).gameObject.SetActive(false);
                remoteVolumesParent.transform.GetChild(child).gameObject.SetActive(false);


                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                /*foreach (Transform child in POINTS)
                {
                    Destroy(child.gameObject);
                }*/

                startEnd.getStartTime = true;
                percentageString = "Error computing: Assertion failed!";
                writeResult = true;
            }

            //throw new System.ArgumentException("Can't generate hull, points are coplanar");
            //throw new UnityEngine.Assertions.AssertionException("Assertion failed", "");

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
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123); //Volume do tetraedro
    }

    float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        //Debug.Log("nr vertices in function: " + vertices.Length);
        //Debug.Log("nr triangles in function: " + triangles.Length);

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }


}