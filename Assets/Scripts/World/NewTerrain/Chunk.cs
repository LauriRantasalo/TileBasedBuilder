using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public Vector2 position;
    public Tile[,] tiles;
    public MeshData meshData;
    public Chunk(Vector2 position)
    {
        this.position = position;
        tiles = new Tile[World.chunkSizeX, World.chunkSizeY];
    }

    public void MergeChunkMesh()
    {

    }
    public void CreateVisualMesh()
    {
        
    }
}
