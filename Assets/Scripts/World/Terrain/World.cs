using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviour
{
    // This seems bad
    public static World instance;
    /// <summary>
    /// How many tiles in a chunk X
    /// </summary>
    public static int chunkSizeX = 16;
    /// <summary>
    /// How many tiles in a chunk Y
    /// </summary>
    public static int chunkSizeY = 16;

    /// <summary>
    /// How many chunks in X
    /// </summary>
    public static int chunkGridSizeX = 3;
    /// <summary>
    /// How many chunks in Y
    /// </summary>
    public static int chunkGridSizeY = 3;

    public Chunk[,] chunks;

    public Material textureMaterial;
    public Sprite grassSprite;
    public Sprite floorSprite;
    public Sprite roadSprite;
    public Sprite wallSprite;

    public GameObject wallCubeGo;
    public Material wallMaterial;

    Builder builder;
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
        builder = GetComponent<Builder>();
        GetComponent<UIHandler>().enabled = true;
        GetComponent<PathFindingGrid>().enabled = true;
        GetComponent<CharactersManager>().enabled = true;
    }

    private void GenerateChunks()
    {
        chunks = new Chunk[chunkGridSizeX, chunkGridSizeY];
        for (int cx = 0; cx < chunkGridSizeX; cx++)
        {
            for (int cy = 0; cy < chunkGridSizeY; cy++)
            {
                Chunk chunk = new Chunk(new Vector2Int(cx, cy));
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
                            if (x % 2 == 0)
                            {
                                chunk.tiles[x, y] = Tile.grass;
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
    }

    List<Vector2> wallPositions;
    List<Vector2> newPositions;
    Vector2 firstPosition;
    bool continueLooping = true;
    int loop = 0;
    public void CheckForRooms(Vector2 position)
    {
        foreach (GameObject item in gos)
        {
            Destroy(item);
        }
        continueLooping = true;
        wallPositions = new List<Vector2>();
        firstPosition = position;
        Vector2 currentPosition = position;
        loop = 0;
        while (continueLooping)
        {
            newPositions = new List<Vector2>();
            CheckSurroundingTiles(currentPosition);


            while (newPositions.Count > 0)
            {
                for (int i = 0; i < newPositions.Count; i++)
                {
                    CheckSurroundingTiles(newPositions[i]);

                }
                newPositions.Clear();
            }

            if (newPositions.Count < 1)
            {
                continueLooping = false;
            }
            // This is just for debuging so it wont crash all the time.
            if (loop > 1000)
            {
                Debug.Log("probably infinite loop");
                continueLooping = false;
            }
            loop++;
        }

    }

    List<GameObject> gos = new List<GameObject>();
    private void CheckSurroundingTiles(Vector2 currentPosition)
    {
        //Debug.Log("first current: " + currentPosition);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 loopingPosition = currentPosition;
                loopingPosition += new Vector2(x, y);
                //Debug.Log(" first looping: " + loopingPosition);

                Vector2 chunkPosition = builder.WorldGridToChunkPos(loopingPosition);
                //Debug.Log("chunk: " + chunkPosition);

                Vector2 inChunkLoopingPosition = new Vector2((int)loopingPosition.x % chunkSizeX, (int)loopingPosition.y % chunkSizeY);
                //Debug.Log("second looping: " + loopingPosition);

                //Debug.Log("current: " + currentPosition);
                //Debug.Log("chunk: " + chunkPosition);
                Chunk chunk = chunks[(int)chunkPosition.x, (int)chunkPosition.y];

                if (chunk.tiles[(int)inChunkLoopingPosition.x, (int)inChunkLoopingPosition.y] == Tile.wall)
                {

                    if (!wallPositions.Contains(loopingPosition)) //&& (x != 0 && y != 0)
                    {
                        
                        if (!newPositions.Contains(loopingPosition))
                        {
                            newPositions.Add(loopingPosition);
                        }
                        else
                        {
                            //Debug.Log("added to wallpositions: " + loopingPosition);
                            wallPositions.Add(loopingPosition);
                            //GameObject temp = Instantiate(wallCubeGo, new Vector3(loopingPosition.x + 0.5f, 4, loopingPosition.y + 0.5f), Quaternion.identity);
                            //gos.Add(temp);
                        }
                    }
                    else
                    {
                        // TODO: Check if you can loop the walls
                    }
                    

                    if (wallPositions.Count > 0 && loopingPosition == wallPositions[0] && loop > 30)
                    {
                        Debug.Log("Found room");
                        continueLooping = false;
                    }

                    //Debug.Log("wallpositions[0]: " + wallPositions[0]);
                    //Debug.Log("looping: " + loopingPosition);

                }
            }

        }
    }
   
    /*
     * 
     * 
 
        for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                }
            }
     * */
    // Update is called once per frame
    void Update()
        {
        }
}

