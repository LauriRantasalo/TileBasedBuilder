using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Chunk
{
    public GameObject chunkGameObject;
    public GameObject wallGameObject;


    public Vector2 position;
    public Tile[,] tiles;
    public MeshData chunkMeshData;
    public MeshData wallMeshData;

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
    

    public void MergeWallMesh(Mesh newWallMesh)
    {
        wallMeshData = new MeshData();
        for (int x = 0; x < chunkSizeX; x++)
        {
            for (int y = 0; y < chunkSizeY; y++)
            {
                if (tiles[x,y] == Tile.wall)
                {
                    wallMeshData.MergeWalls(new MeshData(newWallMesh.vertices, newWallMesh.uv, newWallMesh.triangles), new Vector2(x, y));
                    //wallMeshData.AddWallOffset(new Vector2(x, y));
                }
            }
        }
    }

    public void CreateVisualWallMesh(Mesh wallMesh)
    {
        mesh = wallMeshData.CreateMesh(wallMesh);

        if (wallGameObject == null)
        {
            wallGameObject = new GameObject("WallMesh", typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer));
            wallGameObject.transform.position = new Vector3(position.x * chunkSizeX, 0, position.y * chunkSizeY);
        }
        wallGameObject.GetComponent<MeshRenderer>().material = World.instance.textureMaterial;
        wallGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        wallGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void SetTiles(Vector2[] positions, Tile tile)
    {
        foreach (var pos in positions)
        {
            tiles[(int)pos.x, (int)pos.y] = tile;
        }
    }
    public void MergeChunkMesh()
    {
        chunkMeshData = new MeshData();
        for (int x = 0; x < chunkSizeX; x++)
        {
            for (int y = 0; y < chunkSizeY; y++)
            {
                chunkMeshData.MergeTiles(tiles[x, y].GetMeshData(tiles, new Vector2(x,y)));
            }
        }
    }
    public void CreateVisualChunkMesh(Mesh chunkMesh)
    {
        mesh = chunkMeshData.CreateMesh(chunkMesh);

        if (chunkGameObject == null)
        {
            chunkGameObject = new GameObject("ChunkMesh", typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer));
        }
        chunkGameObject.transform.position = new Vector3(position.x * chunkSizeX, 0, position.y * chunkSizeY);
        chunkGameObject.GetComponent<MeshRenderer>().material = World.instance.textureMaterial;
        chunkGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        chunkGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        chunkGameObject.layer = LayerMask.NameToLayer("TileMask");

    }
}
