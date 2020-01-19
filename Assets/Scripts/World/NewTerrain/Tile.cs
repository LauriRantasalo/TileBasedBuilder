using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static Tile grass = new Tile(World.instance.grassSprite);
    public static Tile floor = new Tile(World.instance.floorSprite);
    public static Tile road = new Tile(World.instance.roadSprite);
    public static Tile wall = new Tile(World.instance.wallSprite);

    public static List<Tile> tileTypes = new List<Tile>() { grass, floor, road, wall };
    //public static List<string> stringTileTypes = new List<string>() { "grass", "floor", "road"};
    
    public Sprite sprite = World.instance.grassSprite;
    //public Vector2 position;
    public Tile(){  }

    public Tile(Sprite sprite)
    {
        this.sprite = sprite;
    }
    public MeshData GetMeshData(Tile[,] tiles, Vector2 position)
    {
        //this.position = position;
        MeshData md = new MeshData();

        md.MergeTiles(new MeshData(
            new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(1, 1, 1) },
            sprite.uv,//new Vector2[] { new Vector2(0,0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1)},
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
