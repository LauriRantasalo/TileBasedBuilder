using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public GameObject charPrefab;

    GameObject character;
    public GameObject target;
    PathFindingGrid pathFindingGrid;


    public void FindNewPaths()
    {
        foreach (GameObject c in characters)
        {
            updatePool.Add(c);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pathFindingGrid = GetComponent<PathFindingGrid>();
        for (int i = 0; i < 50; i++)
        {
            int x = Random.Range(0, World.chunkSizeX * World.chunkGridSizeX);
            int y = Random.Range(0, World.chunkSizeY * World.chunkGridSizeY);
            GameObject temp = Instantiate(charPrefab, new Vector3(x, 1, y), Quaternion.identity);
            characters.Add(temp);
        }

        
        FindNewPaths();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(pathFindingGrid.NodeFromWorldPos(target.transform.position).position + new Vector3(0.5f, 0, 0.5f), Vector3.one);
    }
    Vector3 targetPos = Vector3.zero;

    List<GameObject> updatePool = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(target.transform.position, targetPos) > 0.5f)
        {
            targetPos = target.transform.position;
            updatePool.Clear();
            updatePool.AddRange(characters);
        }

        if (updatePool.Count > 0)
        {
            PathFinding pf = updatePool[0].GetComponent<PathFinding>();
            pf.FindPathFunction(target);
            StartCoroutine(pf.FindPath());

            updatePool.RemoveAt(0);
        }
    }
}
