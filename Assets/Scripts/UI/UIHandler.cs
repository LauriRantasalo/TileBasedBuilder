using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Generation.Terrain;

public class UIHandler : MonoBehaviour
{
    public Dropdown dropdown;
    WorldMain world;
    public int selectedMaterialIndex = 0;
    List<String> tileTypes = new List<string>() { "grass", "floor", "road", "wall" };
    void Start()
    {
        world = GetComponent<WorldMain>();
        //selectedTileType = Tile.grass;
        dropdown.AddOptions(tileTypes);
    }

    public void Dropdown_IndexChanged(int index)
    {
        selectedMaterialIndex = index;
    }

}
