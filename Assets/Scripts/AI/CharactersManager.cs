using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public GameObject charPrefab;

    GameObject character;
    public GameObject target;



    public void FindNewPaths()
    {
        PathFinding pf;
        foreach (GameObject c in characters)
        {
            pf = c.GetComponent<PathFinding>();
            pf.target = target;
            pf.FindPath();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        character = Instantiate(charPrefab, new Vector3(2, 1, 2), Quaternion.identity);
        characters.Add(character);
        character = Instantiate(charPrefab, new Vector3(2, 1, 20), Quaternion.identity);
        characters.Add(character);
        FindNewPaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
