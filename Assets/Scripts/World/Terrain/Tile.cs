using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static Tile floor = new Tile(World.instance.floorSprite);
    public static Tile grass = new Tile(World.instance.grassSprite);
    public static Tile wall = new Tile(World.instance.wallSprite);
    public static Tile road = new Tile(World.instance.roadSprite);
    public static List<Tile> tileTypes = new List<Tile>() { grass, floor, road, wall };
    
    public Sprite sprite = World.instance.grassSprite;
    public Tile(){  }

    public Tile(Sprite sprite)
    {
        this.sprite = sprite;
    }
    
    public MeshData GetMeshData(Tile[,] tiles, Vector2 position)
    {
        MeshData md = new MeshData();

        md.Merge(new MeshData(
            new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(1, 1, 1) },
            new Vector2[] { sprite.uv[1], sprite.uv[2], sprite.uv[3], sprite.uv[0]},    //new Vector2[] { new Vector2(spritePos.x * spriteSize, spritePos.y* spriteSize), new Vector2(spritePos.x * spriteSize, spritePos.y * spriteSize + spriteSize), new Vector2(spritePos.x * spriteSize + spriteSize, spritePos.y * spriteSize), new Vector2(spritePos.x * spriteSize + spriteSize, spritePos.y * spriteSize + spriteSize) }, //sprite.uv,
            new int[] { 0, 1, 2, 3, 2, 1 }
            ));

        md.AddTileOffset(position);
        return md;
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }
}
