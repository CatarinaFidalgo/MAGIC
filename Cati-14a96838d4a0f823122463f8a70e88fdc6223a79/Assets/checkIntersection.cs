using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructiveSolidGeometry;
using GK;
using Oculus.Platform.Samples.VrHoops;
using System.Linq;
using System;

[System.Serializable]
public class checkIntersection : MonoBehaviour
{
    public ConvexHullTryRemote chremote;
    public ConvexHullTry chlocal;

    public GameObject a;
    public GameObject b;
    public List<Vector3> intersectionPoints = new List<Vector3>();
    public float volumeOfUnion;
    public float volumeOfIntersection;
    //public Transform ball;
    public GameObject initialMeshCSG;
    public List<Vector3> ballsList = new List<Vector3>();
    //public Transform POINTS;
    public int child;
    public float percentageOfIntersection;
    //public bool intersectionDone = false;
    public HashSet<Vector3> unionHash = new HashSet<Vector3>();
    public List<Vector3> unionList = new List<Vector3>();
    public bool writeResult;
    public string percentageString;

    public Evaluation eval;
    public StartEndLogs startEnd;


    void Update()
    {
        //writeResult = false;
        
        if (chremote.nrCHremote == chlocal.nrCHlocal && chremote.nrCHremote != 0 && chlocal.nrCHlocal != 0 && chlocal.readyForIntersectionLocal && chremote.readyForIntersectionRemote && eval.localIsDemonstrator)
        {
            child = chremote.nrCHremote - 1;
            Debug.Log("Initiate Intersection");  
            try
            {
                //Debug.Log("Initiate Intersection");                
                //Parametros necessarios para o algoritmo de Convex Hull

                var calc = new ConvexHullCalculator();
                var verts = new List<Vector3>();
                var tris = new List<int>();
                var normals = new List<Vector3>();

                //Debug.Log("UnionHash Int: " + unionHash.Count);

                unionList = unionHash.ToList(); //Converte o hashset para uma lista

               Debug.Log("Union List: " + unionList.Count);  

                calc.GenerateHull(unionList, true, ref verts, ref tris, ref normals);                

                //Create an initial transform that will evolve into our Convex Hull when altering the mesh
                initialMeshCSG.SetActive(true);

                var initialHull = Instantiate(initialMeshCSG);

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

                volumeOfUnion = VolumeOfMesh(mesh) * 1000000; //convert to cm3
                //Debug.Log("Volume of Union: " + volumeOfUnion);

                //Limpar os pontos antigos da lista para o proximo convex hull e
                //informar o programa de que já realizou esta função 

                volumeOfIntersection = VolumeOfIntersection(volumeOfUnion, chremote.volume, chlocal.volume);
                //Debug.Log("Volume of Intersection" + volumeOfIntersection);

                //Compare the volume of intersection with the volume that the local pointed

                percentageOfIntersection = volumeOfIntersection / chlocal.volume * 100.0f;
                Debug.Log("Percentage of Intersection: " + percentageOfIntersection.ToString("F0") + "%");

                //Debug.Log(" V_Loc: " + volumeLocal.ToString("F0") + " V_Rem: " + volumeRemote.ToString("F0") + " V_Union: " + volumeOfUnion.ToString("F0") + " V_Int: " + volumeOfIntersection.ToString("F0"));
                percentageString = chlocal.volume.ToString() + '#' + chremote.volume.ToString() + '#' + volumeOfUnion.ToString() + '#' + volumeOfIntersection.ToString() + '#' + percentageOfIntersection.ToString("F1");
                writeResult = true;

                //ballsList.Clear();
                unionHash.Clear();
                unionList.Clear();

                initialMeshCSG.SetActive(false);
                
                
                a.transform.GetChild(child).gameObject.SetActive(true);
                b.transform.GetChild(child).gameObject.SetActive(true);                
                initialHull.SetActive(true);

                

                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                Debug.Log("Intersexction write = " + writeResult);

            }

            catch (System.ArgumentException)
            {
                //ballsList.Clear();
                unionHash.Clear();
                unionList.Clear();

                initialMeshCSG.SetActive(false);
                a.transform.GetChild(child).gameObject.SetActive(false);
                b.transform.GetChild(child).gameObject.SetActive(false);

                /*initialMeshCSG.SetActive(false);
                a.SetActive(false);
                b.SetActive(false);*/

                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                /*foreach (Transform child in POINTS)
                {
                    Destroy(child.gameObject);
                }*/

                startEnd.getStartTime = true;

            }

            catch (UnityEngine.Assertions.AssertionException)
            {
                //ballsList.Clear();
                unionHash.Clear();
                unionList.Clear();

                initialMeshCSG.SetActive(false);
                a.transform.GetChild(child).gameObject.SetActive(false);
                b.transform.GetChild(child).gameObject.SetActive(false);

                /*initialMeshCSG.SetActive(false);
                a.SetActive(false);
                b.SetActive(false);*/

                chlocal.readyForIntersectionLocal = false;
                chremote.readyForIntersectionRemote = false;

                /*foreach (Transform child in POINTS)
                {
                    Destroy(child.gameObject);
                }*/

                startEnd.getStartTime = true;
            }







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

    float VolumeOfIntersection(float volumeOfUnion, float volumeRemote, float volumeLocal)
    {
        float volumeOfIntersection;

        if (volumeOfUnion < 0.0f)
        {
            throw new System.ArgumentException("Error computing Union.Please Try again.");
            
        }

        
        volumeOfIntersection = volumeLocal + volumeRemote - volumeOfUnion;

        if (volumeOfIntersection <= 0.0f)
        {
            volumeOfIntersection = 0;//- volumeOfIntersection;
        }

        return volumeOfIntersection;
    }

    List<Vector3> CSGtoListOfPoints(List<Polygon> polygons)
    {
        HashSet<Vector3> hash = new HashSet<Vector3>();
        int i = 0;

        for (int pi = 0; pi < polygons.Count; pi++)
        {
            for (int v = 0; v < polygons[pi].vertices.Length; v++)
            {
                hash.Add(polygons[pi].vertices[v].pos);
                i++;
            }

        }

        List<Vector3> points = hash.ToList();

        return points;
    }
}


/*public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 o)
     {
         Vector3 v1 = p1 - o;
         Vector3 v2 = p2 - o;
         Vector3 v3 = p3 - o;
 
         return Vector3.Dot(Vector3.Cross(v1, v2), v3) / 6f; ;
     }
 
     public float VolumeOfMesh(Mesh mesh)
     {
         float volume = 0;
         Vector3[] vertices = mesh.vertices;
         int[] triangles = mesh.triangles;
 
         Vector3 o = new Vector3(0f, 0f, 0f);
         // Computing the center mass of the polyhedron as the fourth element of each mesh
         for (int i = 0; i < mesh.triangles.Length; i++)
         {
             o += vertices[triangles[i]];
         }
         o = o / mesh.triangles.Length;
 
         // Computing the sum of the volumes of all the sub-polyhedrons
         for (int i = 0; i < mesh.triangles.Length; i += 3)
         {
             Vector3 p1 = vertices[triangles[i + 0]];
             Vector3 p2 = vertices[triangles[i + 1]];
             Vector3 p3 = vertices[triangles[i + 2]];
             volume += SignedVolumeOfTriangle(p1, p2, p3, o);
         }
         return Mathf.Abs(volume);
     }
     
     //The method above is correct for "simple" objects (no intersecting/overlapping triangles) 
     //like spheres tetrahedras and so on. For more complex shapes, a good idea could be to segment
     //the mesh (close it) and calculate the volume of each segment separately. Hope this helps.
     
     
     
     */
