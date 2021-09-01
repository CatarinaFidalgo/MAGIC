using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    public Transform target;
    public float radius = 0.5f;

    private MeshRenderer _meshRenderer;
    private Material _material;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = Resources.Load("Materials/Highlight") as Material;
        _meshRenderer.material = _material;

    }

    // Update is called once per frame
    void Update()
    {
        _meshRenderer.material.SetVector("_TargetPos", target.position);
        _meshRenderer.material.SetFloat("_Radius", radius);
    }
}
