using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviour
{
    // This seems bad
    public static World instance;
    public static int chunkSizeX = 16;
    public static int chunkSizeY = 16;

    public static int chunkGridSizeX = 3;
    public static int chunkGridSizeY = 3;

    public Chunk[,] chunks;

    public Material textureMaterial;
    public Sprite grassSprite;
    public Sprite floorSprite;
    public Sprite roadSprite;
    public Sprite wallSprite;

    public GameObject wallCubeGo;
    public Material wallMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // This seems even worse
        World.instance = this;

        GenerateChunks();
        foreach (Chunk chunk in chunks)
        {
            chunk.MergeChunkMesh();
            chunk.CreateVisualChunkMesh(new Mesh());
        }
        GetComponent<Builder>().enabled = true;
        GetComponent<UIHandler>().enabled = true;
    }

    //this should be in chunk
    
    private void GenerateChunks()
    {
        chunks = new Chunk[chunkGridSizeX, chunkGridSizeY];
        for (int cx = 0; cx < chunkGridSizeX; cx++)
        {
            for (int cy = 0; cy < chunkGridSizeY; cy++)
            {
                Chunk chunk = new Chunk(new Vector2(cx, cy));
                chunks[cx, cy] = chunk;
                for (int x = 0; x < chunkSizeX; x++)
                {
                    for (int y = 0; y < chunkSizeY; y++)
                    {
                        if (x == 15 && y == 15)
                        {
                            chunk.tiles[x, y] = Tile.floor;

                        }
                        else
                        {
                            chunk.tiles[x, y] = Tile.grass;
                        }
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
