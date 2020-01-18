using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


    /*
public class Chunk
{
    public GameObject gameObject;
    public Vector3 worldPosition;
    public Vector2 gridPosition;
    public Tile[,] tiles;
    public Mesh chunkMesh;
    public Chunk(Vector3 worldPos, Vector2 gridPos, Tile[,] chunkTiles, Mesh mesh, GameObject go)
    {
        worldPosition = worldPos;
        gridPosition = gridPos;
        tiles = chunkTiles;
        chunkMesh = mesh;
        gameObject = go;
    }
}
public class Tile
{
    public Vector2 gridPosition;
    public int type;

    MeshData meshData = new MeshData();
    public Tile(Vector2 gridPos, int tileType)
    {
        gridPosition = gridPos;
        type = tileType;
    }
    public Tile()
    {

    }
    public MeshData GetMeshData(Tile[,] tiles, Vector2 pos)
    {
        return meshData.CreateTileMesh(tiles, pos);
    }

    
}
public class WorldLoader : MonoBehaviour
{
    public GameObject worldEmpty;
    public GameObject gridCubeGo;

    [HideInInspector]
    public GameObject gridCube;
    [HideInInspector]
    public GameObject selectionStartTile;
    [HideInInspector]
    public GameObject selectionEndTile;


    public static int chunkSizeX = 16;
    public static int chunkSizeY = 16;

    public static int chunkGridSizeX = 2;
    public static int chunkGridSizeY = 2;

    public Chunk[,] chunks;
    public Tile[,] chunkTiles;

    MeshData meshData;
    WorldBuilder worldBuilder;

    Mesh chunkMesh;
    Vector3[] verts;
    Vector2[] uvs;
    int[] tris;

    public Material spriteSheet;
    public Sprite grassSprite;
    public Sprite floorSprite;
    public Sprite debugSprite;

    void Start()
    {
        
        chunks = new Chunk[chunkGridSizeX, chunkGridSizeY];
        chunkMesh = new Mesh();
        worldBuilder = GetComponent<WorldBuilder>();
        meshData = new MeshData();
        chunkTiles = new Tile[chunkSizeX, chunkSizeY];

        gridCube = Instantiate(gridCubeGo, new Vector3(0, -2, 0), Quaternion.identity);
        gridCube.name = "GridCube";
        gridCube.SetActive(false);

        selectionStartTile = Instantiate(gridCubeGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionStartTile.name = "SelectionStartTile";
        selectionStartTile.SetActive(false);

        selectionEndTile = Instantiate(gridCubeGo, new Vector3(0, -2, 0), Quaternion.identity);
        selectionEndTile.name = "SelectionEndTile";
        selectionEndTile.SetActive(false);

        CreateChunks();
        GetComponent<WorldBuilder>().enabled = true;
        GetComponent<UIHandler>().enabled = true;
    }


    public Sprite GetSprite(int tileType)
    {
        switch (tileType)
        {
            case 0:
                return grassSprite;
            case 1:
                return floorSprite;
            default:
                return debugSprite;
        }
    }




    void CreateChunks()
    {

        for (int cx = 0; cx < chunkGridSizeX; cx++)
        {
            for (int cy = 0; cy < chunkGridSizeY; cy++)
            {
                chunkMesh = new Mesh();
                verts = null;
                uvs = null;
                tris = null;

                for (int x = 0; x < chunkSizeX; x++)
                {
                    for (int y = 0; y < chunkSizeY; y++)
                    {
                        Sprite s = grassSprite;
                        /*
                         * if (cx == 1)
                        {
                            s = floorSprite;
                        }
                         * 
                        Mesh tileMesh = meshData.CreateTileMesh(new Vector2(x, y), s);
                        chunkTiles[x, y] = new Tile(new Vector2(x, y), tileMesh, 0);

                        if (verts == null)
                        {
                            verts = tileMesh.vertices;
                            tris = tileMesh.triangles;
                            uvs = tileMesh.uv;
                            continue;
                        }
                        int count = verts.Length;
                        ArrayUtility.AddRange(ref verts, tileMesh.vertices);
                        ArrayUtility.AddRange(ref uvs, tileMesh.uv);

                        for (int i = 0; i < tileMesh.triangles.Length; i++)
                        {
                            ArrayUtility.Add(ref tris, tileMesh.triangles[i] + count);
                        }
                    }
                }


                GameObject chunkGo = new GameObject("WorldMesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

                chunkMesh.vertices = verts;
                chunkMesh.uv = uvs;
                chunkMesh.triangles = tris;

                Chunk chunk = new Chunk(new Vector3(cx * chunkSizeX, 0, cy * chunkSizeY),new Vector2(cx, cy), chunkTiles, chunkMesh, chunkGo);

                chunkGo.layer = 9;
                chunkGo.transform.position = chunk.worldPosition;
                chunkGo.GetComponent<MeshFilter>().mesh = chunkMesh;
                chunkGo.GetComponent<MeshCollider>().sharedMesh = chunkMesh;


                chunkGo.GetComponent<MeshRenderer>().material = spriteSheet;


                chunks[cx, cy] = chunk;


            }
        }
    }


}
}

*/