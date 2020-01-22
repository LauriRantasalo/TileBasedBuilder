using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                Vector2 chunkPos = builder.WorldGridToChunkPos(worldPosition);
                Chunk chunk = world.chunks[(int)chunkPos.x, (int)chunkPos.y];
                Vector2 inChunkPosition = new Vector2(worldPosition.x % World.chunkSizeX, worldPosition.z % World.chunkSizeY);
                if (chunk.tiles[(int)inChunkPosition.x,(int)inChunkPosition.y] == Tile.wall)
                {
                    grid[x, y] = new Node(true, worldPosition, x, y);
                }
                else
                {
                    grid[x, y] = new Node(false, worldPosition, x, y);

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

    
}
