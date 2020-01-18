using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public static Tile grass = new Tile(false);
    public static Tile floor = new Tile(false);
    public static Tile road = new Tile(false);

    public bool isTransparrent;
    FaceCalculator fc = new FaceCalculator();

    public Tile()
    {

    }

    public Tile(bool isTransparrent)
    {
        this.isTransparrent = isTransparrent;
    }
    public MeshData GetMeshData(Tile[,] tiles, Vector2 position)
    {
        return fc.DrawTile(tiles, this, position);
    }
}
