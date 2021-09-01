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
    public GameObject initialMeshCSG;
    public List<Vector3> ballsList = new List<Vector3>();
    public Transform POINTS;
    //public bool readyRemote = true;
    //public bool readylocal = true;

    //public IntersectionCSGTry intersectionCSG;
    void Start()
    {
        //Intersection through CSG

        //return;
        
        
        CSG A = CSG.fromMesh(a.GetComponent<MeshFilter>().mesh, a.transform);
        CSG B = CSG.fromMesh(b.GetComponent<MeshFilter>().mesh, b.transform);

        //CSG A = CSG.fromMesh(a.GetComponent<MeshFilter>().mesh, a.transform.GetChild(0));
        //CSG B = CSG.fromMesh(b.GetComponent<MeshFilter>().mesh, b.transform.GetChild(0));

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

        volume = VolumeOfMesh(mesh) * 1000000; //convert to cm3
        Debug.Log("Volume: " + volume);

        //Limpar os pontos antigos da lista para o proximo convex hull e
        //informar o programa de que já realizou esta função 

        //intersectionCSG.intersectionPoints.Clear();

        initialMeshCSG.SetActive(false);
        a.SetActive(false);
        b.SetActive(false);
        

        
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

    




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstructiveSolidGeometry;
using GK;
using Oculus.Platform.Samples.VrHoops;
using System.Linq;
using System;
public class checkIntersection : MonoBehaviour
{
    public ConvexHullTry chremote;
    public ConvexHullTry chlocal;

    public GameObject a;
    public GameObject b;
    public List<Vector3> intersectionPoints = new List<Vector3>();
    public float volumeOfUnion;
    public float volumeOfIntersection;
    public Transform ball;
    public GameObject initialMeshCSG;
    public List<Vector3> ballsList = new List<Vector3>();
    public Transform POINTS;
    public int child;
    public float percentageOfIntersection;
    //public bool intersectionDone = false;
    public HashSet<Vector3> unionPoints = new HashSet<Vector3>();

    void Update()
    {
        
        if (chremote.nrCHremote == chlocal.nrCHlocal && chremote.nrCHremote != 0 && chlocal.nrCHlocal != 0 && chlocal.readyForIntersectionLocal && chremote.readyForIntersectionRemote)
        {
            //Debug.Log("Nr CH remote: " + chremote.nrCHremote + " Nr CH local: " + chlocal.nrCHlocal + " Local ready: " + chlocal.readyForIntersectionLocal + " Remote ready: " + chremote.readyForIntersectionRemote);
            //Debug.Log("Compute Intersection");
            
            //Intersection through CSG

            child = chremote.nrCHremote - 1;        

            CSG A = CSG.fromMesh(a.transform.GetChild(child).GetComponent<MeshFilter>().mesh, a.transform.GetChild(child));
            CSG B = CSG.fromMesh(b.transform.GetChild(child).GetComponent<MeshFilter>().mesh, b.transform.GetChild(child));

            //CSG A = CSG.fromMesh(a.GetComponent<MeshFilter>().mesh, a.transform);
            //CSG B = CSG.fromMesh(b.GetComponent<MeshFilter>().mesh, b.transform);

            if(A.polygons.Count == 0 && B.polygons.Count == 0)
            {
                Debug.Log("Can't compute intersection for local or remote CH are null.");
            }

            else
            {
                //CSG result = A.intersect(B);
                CSG result = A.union(B);

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

                try
                {
                    //Debug.Log("Balls List " + ballsList.Count);

                    calc.GenerateHull(ballsList, true, ref verts, ref tris, ref normals);

                    //Debug.Log("Depois da CH");

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
                    Debug.Log("Volume of Union: " + volumeOfUnion);

                    //Limpar os pontos antigos da lista para o proximo convex hull e
                    //informar o programa de que já realizou esta função 

                    volumeOfIntersection = VolumeOfIntersection(volumeOfUnion, chremote.volume, chlocal.volume);
                    Debug.Log("Volume of Intersection" + volumeOfIntersection);

                    //Compare the volume of intersection with the volume that the local pointed

                    percentageOfIntersection = volumeOfIntersection / chlocal.volume * 100.0f;
                    Debug.Log("Percentage of Intersection" + percentageOfIntersection);

                    ballsList.Clear();

                    initialMeshCSG.SetActive(false);
                    //a.SetActive(false);
                    //b.SetActive(false);

                    chlocal.readyForIntersectionLocal = false;
                    chremote.readyForIntersectionRemote = false;

                    foreach (Transform child in POINTS)
                    {
                        Destroy(child.gameObject);
                    }

                    //Debug.Log("ChildBalls: " + POINTS.childCount);
                }

                catch (System.ArgumentException)
                {
                    ballsList.Clear();

                    initialMeshCSG.SetActive(false);
                    a.SetActive(false);
                    b.SetActive(false);

                    chlocal.readyForIntersectionLocal = false;
                    chremote.readyForIntersectionRemote = false;

                    foreach (Transform child in POINTS)
                    {
                        Destroy(child.gameObject);
                    }
                }

                catch (UnityEngine.Assertions.AssertionException)
                {
                    ballsList.Clear();

                    initialMeshCSG.SetActive(false);
                    a.SetActive(false);
                    b.SetActive(false);

                    chlocal.readyForIntersectionLocal = false;
                    chremote.readyForIntersectionRemote = false;

                    foreach (Transform child in POINTS)
                    {
                        Destroy(child.gameObject);
                    }
                }
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
        Debug.Log("nr vertices in function: " + vertices.Length);
        Debug.Log("nr triangles in function: " + triangles.Length);

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

        if (volumeOfUnion < 1.0f)
        {
            Debug.Log("Error computing Union. Please Try again.");
            return 0;
        }

        if (volumeOfUnion < volumeLocal || volumeOfUnion < volumeRemote)
        {
            if (volumeLocal > volumeRemote)
            {
                volumeOfUnion = volumeLocal;
            }

            else
            {
                volumeOfUnion = volumeRemote;
            }

            
        }

        volumeOfIntersection = volumeLocal + volumeRemote - volumeOfUnion;

        if (volumeOfIntersection <= 0.0f)
        {
            volumeOfIntersection = 0.0f;
        }

        return volumeOfIntersection;
    }

    List<Vector3> CSGtoListOfPoints(List<Polygon> polygons)
    {
        HashSet<Vector3> hash = new HashSet<Vector3>();

        int i = 0;

       // Debug.Log("Number of polygons in result from CGS: " + polygons.Count);

        for (int pi = 0; pi < polygons.Count; pi++)
        {
            for (int v = 0; v < polygons[pi].vertices.Length; v++)
            {
                hash.Add(polygons[pi].vertices[v].pos);
                i++;
            }

        }

        //Debug.Log("Number of points in the HashSet:" + hash.Count);
        //Debug.Log("Number of i:" + i);

        List<Vector3> points = hash.ToList();

        return points;
    }
}


     public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 o)
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

}




