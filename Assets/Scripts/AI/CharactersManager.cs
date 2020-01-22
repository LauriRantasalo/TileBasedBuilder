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
            pf.finalPath = new List<Node>();
            pf.FindPath(pf.transform.position, target.transform.position);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        GameObject temp1 = Instantiate(charPrefab, new Vector3(2, 1, 10), Quaternion.identity);
        characters.Add(temp1);
        temp1 = Instantiate(charPrefab, new Vector3(2, 1, 2), Quaternion.identity);
        characters.Add(temp1);
        FindNewPaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
