using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class PathFindingGrid : MonoBehaviour
{

    public Transform startPosition;
    //public LayerMask wallMask;
    //public Vector2 gridWorldSize;

    //public float nodeRadius;
    //public float distance;

    public Node[,] nGrid;
    public List<Node> nFinalPath;

    //float nodeDiameter;
    public int gridSizeX, gridSizeY;

    Builder builder;
    World world;
    private void Start()
    {
        world = GetComponent<World>();
        builder = GetComponent<Builder>();
        //nodeDiameter = nodeRadius * 2;
        gridSizeX = World.chunkSizeX * World.chunkGridSizeX;//Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = World.chunkSizeY * World.chunkGridSizeY;//Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public Node NodeFromWorldPos(Vector3 worldPos)
    {
        Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.z));
        return nGrid[gridPos.x, gridPos.y];
    }

    public void CreateGrid()
    {
        //grid = new NativeArray<StructNode>(gridSizeX * gridSizeY, Allocator.Persistent);
        nGrid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPosition = new Vector3(x, 0, y);
                Vector2 chunkPos = builder.WorldGridToChunkPos(new Vector2(worldPosition.x, worldPosition.z));
                Chunk chunk = world.chunks[(int)chunkPos.x, (int)chunkPos.y];
                Vector2 inChunkPosition = new Vector2(worldPosition.x % World.chunkSizeX, worldPosition.z % World.chunkSizeY);
                if (chunk.tiles[(int)inChunkPosition.x, (int)inChunkPosition.y] == Tile.wall)
                {
                    nGrid[x, y] = new Node(true, worldPosition, x, y);
                    //grid[y * gridSizeY + x] = new StructNode(true, worldPosition, x, y);
                }
                else
                {
                    nGrid[x, y] = new Node(false, worldPosition, x, y);
                    //grid[y * gridSizeY + x] = new StructNode(false, worldPosition, x, y);

                }
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node currentNode)
    {
        //NativeArray<StructNode> neighborNodes = new NativeArray<StructNode>(4, Allocator.Temp);
        List<Node> neighborNodes = new List<Node>();
        int xCheck;
        int yCheck;

        // Right side
        xCheck = currentNode.gridX + 1;
        yCheck = currentNode.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(nGrid[xCheck, yCheck]);
                //neighborNodes[0] = grid[yCheck * gridSizeY + xCheck];
            }
        }

        // Left side
        xCheck = currentNode.gridX - 1;
        yCheck = currentNode.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(nGrid[xCheck, yCheck]);
                //neighborNodes[1] = grid[yCheck * gridSizeY + xCheck];
            }
        }
        // Top side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(nGrid[xCheck, yCheck]);

            }
        }

        // Bottom side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(nGrid[xCheck, yCheck]);

            }
        }
        return neighborNodes;
    }
}

    /*

public class PathFindingGrid : MonoBehaviour
{










    public Transform startPosition;
    //public LayerMask wallMask;
    //public Vector2 gridWorldSize;

    //public float nodeRadius;
    //public float distance;

    public Node[,] grid;
    public List<Node> finalPath;

    //float nodeDiameter;
    public int gridSizeX, gridSizeY;

    Builder builder;
    World world;
    private void Start()
    {
        world = GetComponent<World>();
        builder = GetComponent<Builder>();
        //nodeDiameter = nodeRadius * 2;
        gridSizeX = World.chunkSizeX * World.chunkGridSizeX;//Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = World.chunkSizeY * World.chunkGridSizeY;//Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public Node NodeFromWorldPos(Vector3 worldPos)
    {
        Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.z));
        return grid[gridPos.x, gridPos.y];
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPosition = new Vector3(x, 0, y);
                Vector2 chunkPos = builder.WorldGridToChunkPos(new Vector2(worldPosition.x, worldPosition.z));
                Chunk chunk = world.chunks[(int)chunkPos.x, (int)chunkPos.y];
                Vector2 inChunkPosition = new Vector2(worldPosition.x % World.chunkSizeX, worldPosition.z % World.chunkSizeY);
                if (chunk.tiles[(int)inChunkPosition.x,(int)inChunkPosition.y] == Tile.wall)
                {
                    grid[x, y] = new Node(true, worldPosition + new Vector3(0.5f, 0, 0.5f), x, y);
                }
                else
                {
                    grid[x, y] = new Node(false, worldPosition + new Vector3(0.5f, 0, 0.5f), x, y);

                }
            }
        }
    }

    public List<Node> GetNeighboringNodes(Node currentNode)
    {
        List<Node> neighborNodes = new List<Node>();
        int xCheck;
        int yCheck;

        // Right side
        xCheck = currentNode.gridX + 1;
        yCheck = currentNode.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Left side
        xCheck = currentNode.gridX - 1;
        yCheck = currentNode.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }
        // Top side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Bottom side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighborNodes.Add(grid[xCheck, yCheck]);
            }
        }
        return neighborNodes;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(gridSizeX / 2, 0, gridSizeY / 2), new Vector3(gridSizeX, 1, gridSizeY));
        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.blue;
                }

                Gizmos.DrawCube(new Vector3(node.gridX + 0.5f, 1, node.gridY + 0.5f), Vector3.one);
            }
        }
    }
}

    */
      