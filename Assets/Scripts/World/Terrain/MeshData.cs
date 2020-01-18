using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace old
{

public class MeshData
{
    Vector3[] verts;
    Vector2[] uvs;
    int[] tris;
    public Vector2 sprite = new Vector2(0, 3);

    float spriteSize = 0.25f;

    public MeshData(Vector3[] verts, Vector2[] uvs, int[] tris)
    {
        this.verts = verts;
        this.uvs = uvs;
        this.tris = tris;
    }
    public MeshData()
    {

    }
    public Mesh UpdateTileSprite(Mesh mesh, Sprite s)
    {
        mesh.uv[0] = s.uv[0];
        mesh.uv[1] = s.uv[1];
        mesh.uv[2] = s.uv[2];
        mesh.uv[3] = s.uv[3];


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public Mesh CreateTileMesh(Vector2 tilePos, Sprite s)
    {

        verts = new Vector3[4];
        uvs = new Vector2[4];
        tris = new int[6];

        verts[0] = new Vector3(0,1,0);
        verts[1] = new Vector3(0,1,1);
        verts[2] = new Vector3(1,1,0);
        verts[3] = new Vector3(1,1,1);


        uvs[0] = s.uv[0];
        uvs[1] = s.uv[1];
        uvs[2] = s.uv[2];
        uvs[3] = s.uv[3];

        // This is how it should work, but unity was being weird I think about this, so Im doing it Unitys way
        //uvs[0] = new Vector2(sprite.x * spriteSize, sprite.y * spriteSize);
        //uvs[1] = new Vector2(sprite.x * spriteSize + spriteSize, sprite.y * spriteSize);
        //uvs[2] = new Vector2(sprite.x * spriteSize, sprite.y * spriteSize + spriteSize);
        //uvs[3] = new Vector2(sprite.x * spriteSize + spriteSize, sprite.y * spriteSize + spriteSize);

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;
        tris[3] = 3;
        tris[4] = 2;
        tris[5] = 1;

        Mesh mesh = new Mesh();

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += new Vector3(tilePos.x, -1, tilePos.y);
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;

        return mesh;
    }

     
}

}
