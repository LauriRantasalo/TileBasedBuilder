using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public static int chunkSizeX = 16;
    public static int chunkSizeY = 16;

    public static int chunkGridSizeX = 2;
    public static int chunkGridSizeY = 2;

    public Chunk[,] chunks;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateChunks();
        foreach (Chunk chunk in chunks)
        {
            chunk.CreateVisualMesh();
        }
    }

    
    private void GenerateChunks()
    {
        for (int cx = 0; cx < chunkGridSizeX; cx++)
        {
            for (int cy = 0; cy < chunkGridSizeY; cy++)
            {


                for (int x = 0; x < chunkSizeX; x++)
                {
                    for (int y = 0; y < chunkSizeY; y++)
                    {
                        Chunk chunk = new Chunk(new Vector2(cx, cy));
                        chunk.tiles[x, y] = Tile.grass;
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
