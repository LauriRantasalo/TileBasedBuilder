using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Generation.Terrain
{
    public class Chunk
    {
        public GameObject chunkGameObject;
        public GameObject wallGameObject;


        public Vector2Int position;
        public Tile[,] tiles;
        public List<Tile> wallsToRemoveAt = new List<Tile>();

        public MeshData chunkMeshData;
        public MeshData wallMeshData;

        Mesh mesh;

        int chunkSizeX;
        int chunkSizeY;
        public Chunk(Vector2Int position)
        {
            this.position = position;

            chunkSizeX = WorldMain.chunkSizeX;
            chunkSizeY = WorldMain.chunkSizeY;

            tiles = new Tile[chunkSizeX, chunkSizeY];

        }

        public void MergeWallMesh(Mesh newWallMesh)
        {
            wallMeshData = new MeshData();
            for (int x = 0; x < chunkSizeX; x++)
            {
                for (int y = 0; y < chunkSizeY; y++)
                {
                    if (tiles[x, y] == Tile.wall)
                    {
                        wallMeshData.Merge(new MeshData(MeshData.AddWallOffset(newWallMesh.vertices, new Vector2Int(x, y)), newWallMesh.uv, newWallMesh.triangles));
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
                wallGameObject.transform.position = new Vector3Int(position.x * chunkSizeX, 1, position.y * chunkSizeY);
            }
            wallGameObject.GetComponent<MeshRenderer>().material = WorldMain.instance.wallMaterial;
            wallGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            wallGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            wallGameObject.transform.localScale = new Vector3(1, 2, 1);
            if (wallMeshData == new MeshData() && wallGameObject.activeSelf)
            {
                wallGameObject.SetActive(false);
            }
            else
            {
                wallGameObject.SetActive(true);
            }
        }


        public void MergeChunkMesh()
        {
            chunkMeshData = new MeshData();
            for (int x = 0; x < chunkSizeX; x++)
            {
                for (int y = 0; y < chunkSizeY; y++)
                {
                    chunkMeshData.Merge(tiles[x, y].GetMeshData(tiles, new Vector2Int(x, y)));
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
            chunkGameObject.GetComponent<MeshRenderer>().material = WorldMain.instance.textureMaterial;
            chunkGameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
            chunkGameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            chunkGameObject.layer = LayerMask.NameToLayer("TileMask");

        }


    }

}
