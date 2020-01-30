using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generation.Terrain;
using Ai.PathFinding;

public class CharacterManager : MonoBehaviour
{

    public GameObject characterPrefab;
    public int nroOfCharactersToSpawn;
    private List<GameObject> characters = new List<GameObject>();

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nroOfCharactersToSpawn; i++)
        {
            GameObject temp = Instantiate(characterPrefab, new Vector3(Random.Range(0, WorldMain.chunkGridSizeX * WorldMain.chunkSizeX), 1, Random.Range(0, WorldMain.chunkGridSizeY * WorldMain.chunkSizeY)), Quaternion.identity);
            temp.GetComponent<PathFinding>().target = target.transform;
            characters.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
