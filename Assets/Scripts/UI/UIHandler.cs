using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    public Dropdown dropdown;
    World world;
    public int selectedMaterialIndex = 0;
    List<String> tileTypes = new List<string>() { "grass", "floor", "road", "wall" };
    void Start()
    {
        world = GetComponent<World>();
        //selectedTileType = Tile.grass;
        dropdown.AddOptions(tileTypes);
    }

    public void Dropdown_IndexChanged(int index)
    {
        selectedMaterialIndex = index;
    }

}
