using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class MeshData
{

    Vector3[] verts;
    Vector2[] uvs;
    int[] tris;


    public MeshData() { }

    public MeshData(Vector3[] verts, Vector2[] uvs, int[] tris)
    {
        this.verts = verts;
        this.uvs = uvs;
        this.tris = tris;
    }

    public void Merge(MeshData m)
    {
        if (m.verts.Length <= 0)
        {
            return;
        }

        if (verts.Length <= 0)
        {
            verts = m.verts;
            uvs = m.uvs;
            tris = m.tris;
        }

        int count = verts.Length;
        ArrayUtility.AddRange(ref verts, m.verts);
        ArrayUtility.AddRange(ref uvs, m.uvs);

        for (int i = 0; i < m.tris.Length; i++)
        {
            ArrayUtility.Add(ref tris, m.tris[i] + count);
        }

        
    }


    public void AddOffset(Vector2 location)
    {
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += new Vector3(location.x, -1, location.y);
        }
    }
    public Mesh CreateMesh(Mesh chunkMesh)
    {
        if (verts.Length <= 0)
        {
            return new Mesh();
        }
        else
        {
            chunkMesh.vertices = verts;
            chunkMesh.triangles = tris;
            chunkMesh.uv = uvs;

            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();
            return chunkMesh;
        }
    }
}
