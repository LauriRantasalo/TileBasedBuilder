using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    World world;
    UIHandler uiHandler;
    PathFindingGrid pathFindingGrid;
    CharactersManager charactersManager;

    public GameObject gridCubeGo;
    public GameObject selectionGridGo;
    public GameObject selectionStartTileGo;
    public GameObject selectionEndTileGo;

    GameObject gridCube, selectionGrid, selectionStartTile, selectionEndTile;
    Vector2 selectionStartWorldPosition;
    Vector2 selectionEndWorldPosition;

    Tile[,] selectedTiles;
    LayerMask tileLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        world = GetComponent<World>();
        uiHandler = GetComponent<UIHandler>();
        pathFindingGrid = GetComponent<PathFindingGrid>();
        charactersManager = GetComponent<CharactersManager>();

        tileLayerMask = LayerMask.GetMask("TileMask");
        gridCube = Instantiate(gridCubeGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionGrid = Instantiate(selectionGridGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionStartTile = Instantiate(selectionStartTileGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionEndTile = Instantiate(selectionEndTileGo, new Vector3(0, -2, 0), Quaternion.identity);

        gridCube.SetActive(false);
        selectionGrid.SetActive(false);
        selectionStartTile.SetActive(false);
        selectionEndTile.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3000, tileLayerMask) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        {
            //Transform hitObj = hit.transform;
            gridCube.transform.position = new Vector3(Mathf.FloorToInt(hit.point.x) + 0.5f, 0, Mathf.FloorToInt(hit.point.z) + 0.5f);
            gridCube.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
              
                //selectedTiles = null;

                selectionGrid.SetActive(false);
                selectionEndTile.SetActive(false);
                selectionEndWorldPosition = Vector2.zero;
                selectionStartWorldPosition = new Vector2(Mathf.FloorToInt(hit.point.x) + 0.5f, Mathf.FloorToInt(hit.point.z) + 0.5f);
                selectionStartTile.transform.position = new Vector3(selectionStartWorldPosition.x, 0, selectionStartWorldPosition.y);
                selectionStartTile.SetActive(true);
            }
            if (Input.GetMouseButton(0) && selectionStartTile.activeSelf)
            {
                // If Building a wall
                if (uiHandler.selectedMaterialIndex == 3)
                {
                    if (Mathf.Abs(hit.point.x - selectionStartWorldPosition.x) > Mathf.Abs(hit.point.z - selectionStartWorldPosition.y))
                    {
                        selectionEndWorldPosition = new Vector2(Mathf.FloorToInt(hit.point.x) + 0.5f, selectionStartWorldPosition.y);
                    }
                    else
                    {
                        selectionEndWorldPosition = new Vector2(selectionStartWorldPosition.x, Mathf.FloorToInt(hit.point.z) + 0.5f);
                    }
                }
                else
                {
                    selectionEndWorldPosition = new Vector2(Mathf.FloorToInt(hit.point.x) + 0.5f, Mathf.FloorToInt(hit.point.z) + 0.5f);
                }
                selectionEndTile.transform.position = new Vector3(selectionEndWorldPosition.x, 0, selectionEndWorldPosition.y);
                selectionEndTile.SetActive(true);

                (selectionGrid.transform.position, selectionGrid.transform.localScale) = SetUpSelection();
                selectionGrid.SetActive(true);

            }
            if (Input.GetMouseButtonUp(0) && selectionStartTile.activeSelf)
            {
                selectedTiles = FindSelectedTiles(FloorToGrid(selectionStartWorldPosition), FloorToGrid(selectionEndWorldPosition));

                selectionStartTile.SetActive(false);
                selectionEndTile.SetActive(false);
                selectionGrid.SetActive(false);

                UpdateSelectedTiles();

                pathFindingGrid.CreateGrid();
                charactersManager.FindNewPaths();

                // If creating a wall
                if (uiHandler.selectedMaterialIndex == 3)
                {
                    world.CheckForRooms(FloorToGrid(selectionEndWorldPosition));
                }
            }
        }
        else if (gridCube.transform.position != new Vector3(0, -2, 0) || gridCube.activeSelf)
        {
            gridCube.transform.position = new Vector3(0, -2, 0);
            gridCube.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectionGrid.activeSelf || selectionStartTile.activeSelf || selectionEndTile.activeSelf)
            {
                selectionGrid.SetActive(false);
                selectionEndTile.SetActive(false);
                selectionStartTile.SetActive(false);
                selectionGrid.transform.position = new Vector3(0, -2, 0);
                selectionStartTile.transform.position = new Vector3(0, -2, 0);
                selectionEndTile.transform.position = new Vector3(0, -2, 0);
            }
        }
    }

    void UpdateSelectedTiles()
    {
        List<Chunk> chunksToUpdate = new List<Chunk>();
        Vector2 startPos = FloorToGrid(selectionStartWorldPosition);

        Vector2 directionV = GetSelectionDirection();
        int directionX = (int)directionV.x;
        int directionY = (int)directionV.y;
       
        for (int x = 0; x < selectedTiles.GetLength(0); x++)
        {
            for (int y = 0; y < selectedTiles.GetLength(1); y++)
            {
                Vector2 tileGridPos = new Vector2(startPos.x + x * directionX, startPos.y + y * directionY);
                Vector2 chunkPos = WorldGridToChunkPos(tileGridPos);

                Chunk chunk = world.chunks[(int)chunkPos.x, (int)chunkPos.y];
                Tile tile = chunk.tiles[(int)tileGridPos.x % World.chunkSizeX, (int)tileGridPos.y % World.chunkSizeY];

                if (!chunksToUpdate.Contains(chunk))
                {
                    chunksToUpdate.Add(chunk);
                }
                if (tile == Tile.wall && Tile.tileTypes[uiHandler.selectedMaterialIndex] != Tile.wall)
                {
                    chunk.wallsToRemoveAt.Add(tile);
                }
                chunk.tiles[(int)tileGridPos.x % World.chunkSizeX, (int)tileGridPos.y % World.chunkSizeY] = Tile.tileTypes[uiHandler.selectedMaterialIndex];//uiHandler.selectedTileType;

            }
        }

        foreach (Chunk c in chunksToUpdate)
        {
            c.MergeChunkMesh();
            c.CreateVisualChunkMesh(c.chunkGameObject.GetComponent<MeshFilter>().mesh);
            // There has to be a better way for this
            if (uiHandler.selectedMaterialIndex == 3)
            {
                c.MergeWallMesh(world.wallCubeGo.GetComponent<MeshFilter>().sharedMesh);
                c.CreateVisualWallMesh(new Mesh());
            }
            else
            {
                if (c.wallsToRemoveAt.Count > 0)
                {
                    c.MergeWallMesh(world.wallCubeGo.GetComponent<MeshFilter>().sharedMesh);
                    c.CreateVisualWallMesh(new Mesh());
                    c.wallsToRemoveAt.Clear();
                }
            }
            
        }

    }

    /// <summary>
    /// Returns center point of selection and the proper scale.
    /// </summary>
    /// <returns></returns>
    (Vector3, Vector3) SetUpSelection()
    {
        Vector2 biggerGridPositions = Vector2.zero;
        Vector2 smallerGridPositions = Vector2.zero;

        Vector2 selectionStartGridPosition = FloorToGrid(selectionStartWorldPosition);
        Vector2 selectionEndGridPosition = FloorToGrid(selectionEndWorldPosition);

        (biggerGridPositions, smallerGridPositions) = GetBiggerAndSmallerGridPositions(selectionStartGridPosition, selectionEndGridPosition);

        Vector2 vectorOfSelectedNodes = new Vector2(biggerGridPositions.x + 1 - smallerGridPositions.x, biggerGridPositions.y + 1 - smallerGridPositions.y);
        Vector3 selectionMiddlePoint = (new Vector3(selectionStartWorldPosition.x - 0.5f, 0, selectionStartWorldPosition.y - 0.5f) + new Vector3(selectionEndWorldPosition.x - 0.5f, 0, selectionEndWorldPosition.y - 0.5f)) / 2;   //(selectionStartPosition + selectionEndPosition) / 2;
        selectionMiddlePoint += new Vector3(0.5f, 0, 0.5f);
        Vector3 selectionLocalScale = new Vector3(vectorOfSelectedNodes.x, 1, vectorOfSelectedNodes.y);

        selectedTiles = new Tile[(int)vectorOfSelectedNodes.x, (int)vectorOfSelectedNodes.y];

        return (selectionMiddlePoint, selectionLocalScale);
    }

    private Tile[,] FindSelectedTiles(Vector2 startGridPos, Vector2 endGridPos)
    {

        Vector2 selectionStartPosChunk = WorldGridToChunkPos(startGridPos);
        Vector2 selectionEndPosChunk = WorldGridToChunkPos(endGridPos);

        Vector2 biggerGridPositions;
        Vector2 smallerGridPositions;

        (biggerGridPositions, smallerGridPositions) = GetBiggerAndSmallerGridPositions(startGridPos, endGridPos);

        for (int x = (int)smallerGridPositions.x; x <= (int)biggerGridPositions.x; x++)
        {
            for (int y = (int)smallerGridPositions.y; y <= (int)biggerGridPositions.y; y++)
            {
                Vector2 chunkGridPos = WorldGridToChunkPos(new Vector2(x, y));
                Chunk chunk = world.chunks[(int)chunkGridPos.x, (int)chunkGridPos.y];
                selectedTiles[x - (int)smallerGridPositions.x, y - (int)smallerGridPositions.y] = chunk.tiles[x - (int)chunk.position.x * World.chunkSizeX, y - (int)chunk.position.y * World.chunkSizeY];
            }
        }
        return selectedTiles;
    }

   
    /// <summary>
    /// If the selections starting X position is smaller than the ending X position 
    /// </summary>
    bool startXSmaller = false;
    /// <summary>
    /// If the selections starting Y position is smaller than the ending Y position
    /// </summary>
    bool startYSmaller = false;
    (Vector2 biggerGridPositions, Vector2 smallerGridPositions) GetBiggerAndSmallerGridPositions(Vector2 selectionStartGridPosition, Vector2 selectionEndGridPosition)
    {
        Vector2 biggerGridPositions;
        Vector2 smallerGridPositions;
        if (selectionStartGridPosition.x >= selectionEndGridPosition.x)
        {
            startXSmaller = false;
            biggerGridPositions.x = selectionStartGridPosition.x;
            smallerGridPositions.x = selectionEndGridPosition.x;
        }
        else
        {
            startXSmaller = true;
            biggerGridPositions.x = selectionEndGridPosition.x;
            smallerGridPositions.x = selectionStartGridPosition.x;
        }

        if (selectionStartGridPosition.y >= selectionEndGridPosition.y)
        {
            startYSmaller = false;
            biggerGridPositions.y = selectionStartGridPosition.y;
            smallerGridPositions.y = selectionEndGridPosition.y;
        }
        else
        {
            startYSmaller = true;
            biggerGridPositions.y = selectionEndGridPosition.y;
            smallerGridPositions.y = selectionStartGridPosition.y;
        }
        return (biggerGridPositions, smallerGridPositions);
    }

    Vector2 GetSelectionDirection()
    {
        int directionX, directionY;
        if (startXSmaller)
        {
            directionX = 1;
        }
        else
        {
            directionX = -1;
        }

        if (startYSmaller)
        {
            directionY = 1;
        }
        else
        {
            directionY = -1;
        }
        return new Vector2(directionX, directionY);
    }
    /// <summary>
    /// Gets the grid position of the chunk that you clicked on
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public Vector2 WorldGridToChunkPos(Vector2 gridPos)
    {
        Vector2 chunkPos = new Vector2(Mathf.FloorToInt(gridPos.x / World.chunkSizeX), Mathf.FloorToInt(gridPos.y / World.chunkSizeY));
        return chunkPos;
    }
    /// <summary>
    ///  Translates the world position to world grid position
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2 FloorToGrid(Vector2 worldPos)
    {
        Vector2 gridPos = new Vector2(worldPos.x - 0.5f, worldPos.y - 0.5f);
        return gridPos;
    }
}
