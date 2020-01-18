using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
namespace old
{

public class WorldBuilder : MonoBehaviour
{
    int tileLayerMask = 9;

    GameObject gridCube;

    GameObject selectionStartTile;
    GameObject selectionEndTile;
    GameObject selectionGrid;

    WorldLoader worldLoader;
    UIHandler uiHandler;
    MeshData meshData;

    Tile[,] selectedTiles;

    public GameObject selectionGridGo;

    public Material grassMaterial;
    public Material floorMaterial;
    public Material pavementMaterial;
    public Material roadMaterial;
    public Material wallMaterial;
    public Dictionary<string, Material> materialDict = new Dictionary<string, Material>();

    // Start is called before the first frame update
    void Start()
    {
        worldLoader = GetComponent<WorldLoader>();
        uiHandler = GetComponent<UIHandler>();
        meshData = new MeshData();

        tileLayerMask = LayerMask.GetMask("TileMask");
        selectionStartTile = worldLoader.selectionStartTile;
        selectionEndTile = worldLoader.selectionEndTile;
        gridCube = worldLoader.gridCube;
        selectionGrid = Instantiate(selectionGridGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionGrid.SetActive(false);

        materialDict.Add("Grass", grassMaterial);
        materialDict.Add("Floor", floorMaterial);
        materialDict.Add("Pavement", pavementMaterial);
        materialDict.Add("Road", roadMaterial);
        materialDict.Add("Wall", wallMaterial);
    }
    
    // Update is called once per frame
    Vector2 selectionStartWorldPosition;
    Vector2 selectionEndWorldPosition;
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
                selectionEndWorldPosition = Vector3.zero;
                selectionStartWorldPosition = new Vector2(Mathf.FloorToInt(hit.point.x) + 0.5f, Mathf.FloorToInt(hit.point.z) + 0.5f);
                selectionStartTile.transform.position = new Vector3(selectionStartWorldPosition.x, 0, selectionStartWorldPosition.y);
                selectionStartTile.SetActive(true);
            }
            if (Input.GetMouseButton(0) && selectionStartTile.activeSelf)
            {
                selectionEndWorldPosition = new Vector2(Mathf.FloorToInt(hit.point.x) + 0.5f, Mathf.FloorToInt(hit.point.z) + 0.5f);
                selectionEndTile.transform.position = new Vector3(selectionEndWorldPosition.x, 0, selectionEndWorldPosition.y);
                selectionEndTile.SetActive(true);

                (selectionGrid.transform.position, selectionGrid.transform.localScale) = SetUpSelection();
                selectionGrid.SetActive(true);
            }
            if (Input.GetMouseButtonUp(0) && selectionStartTile.activeSelf)
            {
                selectedTiles = FindSelectedTiles(WorldToGridPos(selectionStartWorldPosition), WorldToGridPos(selectionEndWorldPosition));

                Debug.Log(selectedTiles.Length);
                selectionStartTile.SetActive(false);
                selectionEndTile.SetActive(false);
                selectionGrid.SetActive(false);

                UpdateSelectedTileSprites();
                //FillSelectedNodesWith(materialDict.Values.ElementAt(uiHandler.selectedMaterialIndex));
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

    

    void UpdateSelectedTileSprites()
    {
        List<Chunk> chunksToUpdate = new List<Chunk>();
        Vector2 startPos = WorldToGridPos(selectionStartWorldPosition);
        for (int x = 0; x < selectedTiles.GetLength(0); x++)
        {
            for (int y = 0; y < selectedTiles.GetLength(1); y++)
            {
                Vector2 tileGridPos = new Vector2(startPos.x + x, startPos.y + y);

                Vector2 chunkPos = GridToChunkPos(tileGridPos);
                Chunk chunk = worldLoader.chunks[(int)chunkPos.x, (int)chunkPos.y];

                if (!chunksToUpdate.Contains(chunk))
                {
                    chunksToUpdate.Add(chunk);
                }

                Tile tile = chunk.tiles[(int)tileGridPos.x % WorldLoader.chunkSizeX, (int)tileGridPos.y % WorldLoader.chunkSizeY];
                int tileType = uiHandler.selectedMaterialIndex;
                tile.type = tileType;

                Debug.Log(chunk.tiles[(int)tileGridPos.x % WorldLoader.chunkSizeX, (int)tileGridPos.y % WorldLoader.chunkSizeY]);
                Sprite sprite = worldLoader.GetSprite(tileType);
                tile.tileMesh = meshData.UpdateTileSprite(tile.tileMesh, sprite);




            }


        }

        foreach (Chunk c in chunksToUpdate)
        {
            c.gameObject.GetComponent<MeshCollider>().sharedMesh = c.chunkMesh;
            c.gameObject.GetComponent<MeshFilter>().sharedMesh = c.chunkMesh;
        }
    }
    /// <summary>
    /// Returns the coordinate of the chunk from where the tile is.
    /// </summary>
    /// <returns></returns>
    Vector2 worldToChunkPos(Vector2 worldPos)
    {
        Vector2 gridPos = new Vector2(worldPos.x - 0.5f, worldPos.y - 0.5f);
        Vector2 chunkPos = new Vector2(Mathf.FloorToInt(gridPos.x / WorldLoader.chunkSizeX), Mathf.FloorToInt(gridPos.y / WorldLoader.chunkSizeY));
        return chunkPos;
    }

    Vector2 GridToChunkPos(Vector2 gridPos)
    {
        Vector2 chunkPos = new Vector2(Mathf.FloorToInt(gridPos.x / WorldLoader.chunkSizeX), Mathf.FloorToInt(gridPos.y / WorldLoader.chunkSizeY));
        return chunkPos;
    }
    Vector2 WorldToGridPos(Vector2 worldPos)
    {
        //Vector2 gridPos = new Vector2(worldPos.x + ((WorldLoader.chunkSizeX / 2)) - 0.5f, worldPos.z + ((WorldLoader.chunkSizeY / 2)) - 0.5f);
        Vector2 gridPos = new Vector2(worldPos.x  - 0.5f, worldPos.y - 0.5f);
        return gridPos;
    }

    /// <summary>
    /// Returns center point of selection and the proper scale.
    /// </summary>
    /// <returns></returns>
    (Vector3, Vector3) SetUpSelection()
    {
        Vector2 biggerGridPositions = Vector2.zero;
        Vector2 smallerGridPositions = Vector2.zero;

        Vector2 selectionStartGridPosition = WorldToGridPos(selectionStartWorldPosition);
        Vector2 selectionEndGridPosition = WorldToGridPos(selectionEndWorldPosition);

        (biggerGridPositions, smallerGridPositions) = GetBiggerAndSmallerGridPositions(selectionStartGridPosition, selectionEndGridPosition);

        Vector2 vectorOfSelectedNodes = new Vector2(biggerGridPositions.x + 1 - smallerGridPositions.x, biggerGridPositions.y + 1 - smallerGridPositions.y);
        Vector3 selectionMiddlePoint = (new Vector3(selectionStartWorldPosition.x - 0.5f, 0, selectionStartWorldPosition.y - 0.5f) + new Vector3(selectionEndWorldPosition.x - 0.5f, 0, selectionEndWorldPosition.y - 0.5f)) / 2;   //(selectionStartPosition + selectionEndPosition) / 2;
        selectionMiddlePoint += new Vector3(0.5f, 0, 0.5f);
        Vector3 selectionLocalScale = new Vector3(vectorOfSelectedNodes.x, 1, vectorOfSelectedNodes.y);

        selectedTiles = new Tile[(int)vectorOfSelectedNodes.x, (int)vectorOfSelectedNodes.y];
       
        return (selectionMiddlePoint, selectionLocalScale);
    }

    

    Tile[,] FindSelectedTiles(Vector2 startGridPos, Vector2 endGridPos)
    {
        Vector2 selectionStartPosChunk = GridToChunkPos(startGridPos);
        Vector2 selectionEndPosChunk = GridToChunkPos(endGridPos);

        Vector2 biggerGridPositions;
        Vector2 smallerGridPositions;

        (biggerGridPositions, smallerGridPositions) = GetBiggerAndSmallerGridPositions(startGridPos, endGridPos);

        for (int x = (int)smallerGridPositions.x; x <= (int)biggerGridPositions.x; x++)
        {
            for (int y = (int)smallerGridPositions.y; y <= (int)biggerGridPositions.y; y++)
            {
                Vector2 chunkGridPos =  GridToChunkPos(new Vector2(x, y));
                Chunk chunk = worldLoader.chunks[(int)chunkGridPos.x, (int)chunkGridPos.y];
                selectedTiles[x - (int)smallerGridPositions.x, y - (int)smallerGridPositions.y] = chunk.tiles[x - (int)chunk.gridPosition.x * WorldLoader.chunkSizeX, y - (int)chunk.gridPosition.y * WorldLoader.chunkSizeY];
            }
        }
        return selectedTiles;
    }





    (Vector2 biggerGridPositions, Vector2 smallerGridPositions) GetBiggerAndSmallerGridPositions(Vector2 selectionStartGridPosition, Vector2 selectionEndGridPosition)
    {
        Vector2 biggerGridPositions;
        Vector2 smallerGridPositions;
        if (selectionStartGridPosition.x >= selectionEndGridPosition.x)
        {
            biggerGridPositions.x = selectionStartGridPosition.x;
            smallerGridPositions.x = selectionEndGridPosition.x;
        }
        else
        {
            biggerGridPositions.x = selectionEndGridPosition.x;
            smallerGridPositions.x = selectionStartGridPosition.x;
        }

        if (selectionStartGridPosition.y >= selectionEndGridPosition.y)
        {
            biggerGridPositions.y = selectionStartGridPosition.y;
            smallerGridPositions.y = selectionEndGridPosition.y;
        }
        else
        {
            biggerGridPositions.y = selectionEndGridPosition.y;
            smallerGridPositions.y = selectionStartGridPosition.y;
        }
        return (biggerGridPositions, smallerGridPositions);
    }
}
}
*/