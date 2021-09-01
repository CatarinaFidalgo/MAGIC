using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Input;

namespace GK
{
	public class ConvexHullTry : MonoBehaviour
	{

		public GameObject initialMesh;
		//public Transform trail;
		public int i = 0;		
		public bool generateHullDone = true;
		public SaveTrailPoints saveTrailPoints;
		public float volume;
		
		public Transform ChParent;
		
		
		public int nrCHlocal = 0;
		
		
		public bool readyForIntersectionLocal = false;
		//public checkIntersection checkInt;

		public bool writeFile = false;

        public Evaluation eval;
		public StartEndLogs startEnd;


		IEnumerator Start()
		{
			
			//Parametros necessarios para o algoritmo de Convex Hull

			var calc = new ConvexHullCalculator();
			var verts = new List<Vector3>();
			var tris = new List<int>();
			var normals = new List<Vector3>();



			//pointsTrailRemote = udpListener.remotePoints;
			
			//Debug.Log(udpListener.remotePoints.Count);
			//Debug.Log(pointsTrailRemote.Count);

			while (true)
			{
				if (saveTrailPoints.pressed == false && generateHullDone == false && eval.localIsDemonstrator)
				{
					

					try
					{	
						calc.GenerateHull(saveTrailPoints.pointsTrail, true, ref verts, ref tris, ref normals);
						writeFile = true;
						//Create an initial transform that will evolve into our Convex Hull when altering the mesh

						var initialHull = Instantiate(initialMesh);
						//initialHull = Instantiate(initialMesh);

						initialHull.transform.SetParent(ChParent, false);
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
															   //Debug.Log("Volume of MESH: " + volume);

						//Debug.Log("Verts: " + verts.Count + " Tris: " + tris.Count + " Normals: " + normals.Count);

						//Send points of the mesh to the hashset containing all points for later Union

						
						/*for (int i = 0; i < saveTrailPoints.pointsTrail.Count; i++)
						{
							checkInt.unionHash.Add(saveTrailPoints.pointsTrail[i]);
						}


                        Debug.Log("UnionHash Local: " + checkInt.unionHash.Count);*/
						

						//Limpar os pontos antigos da lista para o proximo convex hull e
						//informar o programa de que já realizou esta função 

						saveTrailPoints.pointsTrail.Clear();
						//pointsTrailRemote.Clear();
						//udpListener.remotePoints.Clear();

						generateHullDone = true;
						//checkInt.intersectionDone = false;

						
						
						nrCHlocal++;
						readyForIntersectionLocal = true;

						Debug.Log("CH Hull Local");

						initialHull.SetActive(false);

						//Debug.Log("Local write = " + writeFile);

						Debug.Log("nr ch local:" + nrCHlocal);


					}
					catch (System.ArgumentException)
					{
						saveTrailPoints.pointsTrail.Clear();						
						generateHullDone = true;
						startEnd.getStartTime = true;
					}

					catch (UnityEngine.Assertions.AssertionException)
					{
						saveTrailPoints.pointsTrail.Clear();						
						generateHullDone = true;
						startEnd.getStartTime = true;
					}




				}



				yield return new WaitForSeconds(0.5f);
				
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
}
