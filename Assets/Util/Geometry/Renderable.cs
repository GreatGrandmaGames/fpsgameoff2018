using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class Renderable : MonoBehaviour {

    private MeshFilter meshFilter;

    public void Awake()
    {
        Render();
    }

    public void Render()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        meshFilter.sharedMesh = GenerateMesh();
    }

    protected abstract Mesh GenerateMesh();
}
