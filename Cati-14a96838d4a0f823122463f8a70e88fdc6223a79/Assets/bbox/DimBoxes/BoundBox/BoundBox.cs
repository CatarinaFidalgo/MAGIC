using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DimBoxes
{
    [ExecuteInEditMode]
    public class BoundBox : MonoBehaviour
    {
        public enum BoundSource
        {
            meshes,
            boxCollider,
        }

        public BoundSource boundSource = BoundSource.meshes;

        public bool permanent = false; //permanent//onMouseDown

        public bool setupOnAwake = false;

        [Header("Rendering methods")]
        public bool wire_renderer = true;
        [HideInInspector]
        public Color wireColor = new Color(0f, 1f, 0.4f, 0.74f);
        [HideInInspector]
        public bool line_renderer = false;
        [HideInInspector]
        public Object linePrefab;
        [HideInInspector]
        [Range(0.005f, 0.25f)] public float lineWidth = 0.03f;
        [HideInInspector]
        public Color lineColor = Color.red;
        [HideInInspector]
        public int numCapVertices = 0;

        private Bounds bound;
        private Vector3 boundOffset;
        [HideInInspector]
        public Bounds colliderBound;
        [HideInInspector]
        public Vector3 colliderBoundOffset;
        [HideInInspector]
        public Bounds meshBound;
        [HideInInspector]
        public Vector3 meshBoundOffset;

        private Vector3[] corners;

        private Vector3[,] lines = new Vector3[0, 0];

        private Quaternion quat;

        private Camera mcamera;

        private DimBoxes.DrawLines cameralines;

        private LineRenderer[] lineList;

        private MeshFilter[] meshes;

        private Material lineMaterial;

        private Vector3 topFrontLeft;
        private Vector3 topFrontRight;
        private Vector3 topBackLeft;
        private Vector3 topBackRight;
        private Vector3 bottomFrontLeft;
        private Vector3 bottomFrontRight;
        private Vector3 bottomBackLeft;
        private Vector3 bottomBackRight;

        [HideInInspector]
        public Vector3 startingScale;
        private Vector3 previousScale;
        private Vector3 startingBoundSize;
        private Vector3 startingBoundCenterLocal;
        private Vector3 previousPosition;
        private Quaternion previousRotation;


        void Reset()
        {
            meshes = GetComponentsInChildren<MeshFilter>();
            calculateBounds();
            Start();
        }

        void Awake()
        {
            if (setupOnAwake)
            {
                meshes = GetComponentsInChildren<MeshFilter>();
                calculateBounds();
            }
        }

        void Start()
        {
            cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;

            if (!cameralines)
            {
                Debug.LogError("DimBoxes: no camera with DimBoxes.DrawLines in the scene", gameObject);
                return;
            }

            mcamera = cameralines.GetComponent<Camera>();
            previousPosition = transform.position;
            previousRotation = transform.rotation;
            startingBoundSize = bound.size;
            startingScale = transform.localScale;
            previousScale = startingScale;
            startingBoundCenterLocal = transform.InverseTransformPoint(bound.center);
            init();
        }

        public void init()
        {
            //
            if (GetComponents<BoxCollider>() == null) boundSource = BoundSource.meshes;
            setPoints();
            setLines();
            setLineRenderers();
        }

        void LateUpdate()
        {
            if (transform.localScale != previousScale)
            {
                setPoints();
            }
            if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale)
            {
                setLines();
                previousRotation = transform.rotation;
                previousPosition = transform.position;
                previousScale = transform.localScale;
            }
            if(wire_renderer) cameralines.setOutlines(lines, wireColor, new Vector3[0, 0]);
        }

        void calculateBounds()
        {
            quat = transform.rotation;//object axis AABB
            Vector3 locScale = transform.localScale;

            BoxCollider coll = GetComponent<BoxCollider>();
            if (coll)
            {
                GameObject co = new GameObject("dummy");
                co.transform.position = transform.position;
                co.transform.localScale = transform.lossyScale;
                BoxCollider cobc = co.AddComponent<BoxCollider>();
                //quat = transform.rotation;
                cobc.center = coll.center;
                cobc.size = coll.size;
                colliderBound = cobc.bounds;
                DestroyImmediate(co);
                colliderBoundOffset = colliderBound.center - transform.position;
            }

            meshBound = new Bounds();

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.localScale = Vector3.one;

            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh ms = meshes[i].sharedMesh;
                int vc = ms.vertexCount;
                for (int j = 0; j < vc; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        meshBound = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                    }
                    else
                    {
                        meshBound.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                    }
                }
            }
            transform.rotation = quat;
            transform.localScale = locScale;
            meshBoundOffset = meshBound.center - transform.position;
        }

        void setPoints()
        {

            if (boundSource == BoundSource.boxCollider)
            {
                if (colliderBound == null)
                {
                    Debug.LogError("no collider - add collider to " + gameObject.name + " gameObject");
                    return;

                }
                bound = colliderBound;
                boundOffset = colliderBoundOffset;
            }

            else
            {
                bound = meshBound;
                boundOffset = meshBoundOffset;
            }

            bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
            boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);


            topFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
            topFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
            topBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
            topBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
            bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
            bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
            bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
            bottomBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, -1));

            corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
        }

        void setLines()
        {

            Quaternion rot = transform.rotation;
            Vector3 pos = transform.position;
            if (wire_renderer)
            {
                List<Vector3[]> _lines = new List<Vector3[]>();
                //int linesCount = 12;

                Vector3[] _line;
                for (int i = 0; i < 4; i++)
                {
                    //width
                    _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 1] + pos };
                    _lines.Add(_line);
                    //height
                    _line = new Vector3[] { rot * corners[i] + pos, rot * corners[i + 4] + pos };
                    _lines.Add(_line);
                    //depth
                    _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 3 - 4 * (i % 2)] + pos };
                    _lines.Add(_line);

                }
                lines = new Vector3[_lines.Count, 2];
                for (int j = 0; j < _lines.Count; j++)
                {
                    lines[j, 0] = _lines[j][0];
                    lines[j, 1] = _lines[j][1];
                }
            }
            //
            if (line_renderer)
            {
                lineList = GetComponentsInChildren<LineRenderer>();
                if (!lineMaterial) lineMaterial = new Material((linePrefab as GameObject).GetComponent<Renderer>().sharedMaterial);
                if (lineList.Length == 0)
                {
                    lineList = new LineRenderer[12];
                    for (int i = 0; i < 12; i++)
                    {
#if UNITY_EDITOR
                        GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                        go.transform.SetParent(transform);
                        go.transform.position = Vector3.zero;
                        go.transform.rotation = Quaternion.identity;
#else
                        GameObject go = (GameObject)Instantiate(linePrefab, transform, false);
#endif
                        lineList[i] = go.GetComponent<LineRenderer>();
                        lineList[i].material = lineMaterial;
                    }
                }

                lineMaterial = lineList[0].sharedMaterial;

                //Debug.Log(lineList.Length);
                for (int i = 0; i < 4; i++)
                {
                    //width
                    lineList[i].SetPositions(new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 1] + pos });
                    //height
                    lineList[i + 4].SetPositions(new Vector3[] { rot * corners[i] + pos, rot * corners[i + 4] + pos });
                    //depth
                    lineList[i + 8].SetPositions(new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 3 - 4 * (i % 2)] + pos });
                }
            }
           //

        }

        public void setLineRenderers()
        {
            if (lineMaterial) lineMaterial.color = lineColor;
            foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
            {
                lr.startWidth = lineWidth;
                lr.enabled = line_renderer;
                lr.numCapVertices = numCapVertices;
            }
        }

        void OnMouseDown()
        {
            if (permanent) { enabled = true;  return; }
            enabled = !enabled;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying) return;
            init();
        }


#endif

        void OnDrawGizmos()
        {
            if (!wire_renderer) return;
                Gizmos.color = wireColor;
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                Gizmos.DrawLine(lines[i, 0], lines[i, 1]);
            }
        }

    }
}
