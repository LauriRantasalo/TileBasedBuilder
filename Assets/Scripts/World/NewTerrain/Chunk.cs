using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public GameObject gameObject;
    public Vector2 position;
    public Tile[,] tiles;
    public MeshData meshData;

    Mesh mesh;

    int chunkSizeX;
    int chunkSizeY;
    public Chunk(Vector2 position)
    {
        this.position = position;

        chunkSizeX = World.chunkSizeX;
        chunkSizeY = World.chunkSizeY;

        tiles = new Tile[chunkSizeX, chunkSizeY];
        
    }

    public void MergeChunkMesh()
    {
        meshData = new MeshData();
        for (int x = 0; x < chunkSizeX; x++)
        {
            for (int y = 0; y < chunkSizeY; y++)
            {
                meshData.Merge(tiles[x, y].GetMeshData(tiles, new Vector2(x,y)));
            }
        }
    }
    public void CreateVisualMesh(Mesh chunkMesh)
    {
        mesh = meshData.CreateMesh(chunkMesh);

        if (gameObject == null)
        {
            Debug.Log("null");
            gameObject = new GameObject("ChunkMesh", typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer));
        }
        gameObject.transform.position = new Vector3(position.x * chunkSizeX, 0, position.y * chunkSizeY);
        gameObject.GetComponent<MeshRenderer>().material = World.instance.textureMaterial;
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;



    }
}
