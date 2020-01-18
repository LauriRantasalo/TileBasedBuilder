using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    public Dropdown dropdown;
    //WorldBuilder worldBuilder;
    public int selectedMaterialIndex = 0;
    void Start()
    {
        //worldBuilder = GetComponent<WorldBuilder>();
        //dropdown.AddOptions(worldBuilder.materialDict.Keys.ToList<string>());
    }

    public void Dropdown_IndexChanged(int index)
    {
        selectedMaterialIndex = index;
    }

}
