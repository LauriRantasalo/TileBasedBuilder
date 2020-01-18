using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCalculator
{
    public Sprite sprite = World.instance.grassSprite;

    public MeshData DrawTile(Tile[,] tiles, Tile tile, Vector2 position)
    {
        MeshData md = new MeshData();

        if (tile.isTransparrent)
        {
            return md;
        }

        md.Merge(new MeshData(
            new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 0), new Vector3(1, 1, 1) },
            sprite.uv,//new Vector2[] { new Vector2(0,0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1)},
            new int[] { 0, 1, 2, 3, 2, 1 }
            ));

        md.AddOffset(position);
        return md;
    }
}
