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
    

    public static Vector3[] AddWallOffset(Vector3[] wallVerts,Vector2 location)
    {
        for (int i = 0; i < wallVerts.Length; i++)
        {
            wallVerts[i] += new Vector3(location.x + 0.5f, 0, location.y + 0.5f);
        }
        return wallVerts;
    }

    public void AddTileOffset(Vector2 location)
    {
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += new Vector3(location.x, -1, location.y);
        }
    }

    public void Merge(MeshData m)
    {
        if (m.verts == null)
        {
            return;
        }

        if (verts == null)
        {
            verts = m.verts;
            uvs = m.uvs;
            tris = m.tris;
            return;
        }

        int count = verts.Length;
        ArrayUtility.AddRange(ref verts, m.verts);
        ArrayUtility.AddRange(ref uvs, m.uvs);

        for (int i = 0; i < m.tris.Length; i++)
        {
            ArrayUtility.Add(ref tris, m.tris[i] + count);
        }
    }

    public Mesh CreateMesh(Mesh mesh)
    {
        if (verts == null || verts.Length <= 0)
        {
            return new Mesh();
        }
        else
        {
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
