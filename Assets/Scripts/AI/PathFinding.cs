using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;

public class PathFinding : MonoBehaviour
{
    GameObject main;
    PathFindingGrid pathFindingGrid;
    //[HideInInspector]
    public GameObject target;
    public Vector3 targetPosition = new Vector3();
    public Vector3 startPosition = new Vector3();
    //public Transform startPosition;
    //public Transform targetPosition;


    public List<Node> nFinalPath = new List<Node>();
    Node[,] nGrid;


    public float speed = 1f;
    private void Awake()
    {
        main = GameObject.Find("Main");
        pathFindingGrid = main.GetComponent<PathFindingGrid>();
        nGrid = pathFindingGrid.nGrid;
    }
    void Update()
    {
        if (target != null )//&& (nFinalPath == null || nFinalPath.Count < 1))
        {
            if (Vector3.Distance(target.transform.position, targetPosition) > 0.5f)
            {
                targetPosition = target.transform.position;
                startPosition = transform.position;
                //StartCoroutine("FindPath");
            }

        }
        
        if (pathFindingGrid.nGrid != nGrid)
        {
            nGrid = pathFindingGrid.nGrid;
            targetPosition = target.transform.position;
            startPosition = transform.position;
            //StartCoroutine("FindPath");
        }
        #region
        /*
        if (target != null && (finalPathList == null || finalPathList.Count < 1))
        {
            targetPosition = target.transform.position;

            finalPath = new NativeArray<StructNode>(World.chunkSizeX * World.chunkGridSizeX * World.chunkSizeY * World.chunkGridSizeY, Allocator.TempJob);
            NativeArray<int> debugArray = new NativeArray<int>(10, Allocator.TempJob);

            FindPath findPathJob = new FindPath
            {
                startPos = transform.position,
                targetPos = targetPosition,
                grid = grid,
                finalPath = finalPath,
                debugArray = debugArray,
            };
            JobHandle jobHandle = findPathJob.Schedule();
            jobHandle.Complete();
            finalPath = findPathJob.finalPath;
            for (int i = 0; i < debugArray.Length; i++)
            {
                Debug.Log("DEBUG array " + i + ": " + debugArray[i]);

            }
            finalPathList.AddRange(finalPath);
            finalPath.Dispose();
            debugArray.Dispose();
        }
        */
        #endregion
        if (nFinalPath != null && nFinalPath.Count > 0)
        {
            if (pathFindingGrid.NodeFromWorldPos(transform.position) != nFinalPath[0])
            {
                //Debug.Log(pathFindingGrid.NodeFromWorldPos(transform.position).position);
                //Debug.Log(finalPathList[0].position);
                //Debug.Log(finalPathList[2303].position);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(nFinalPath[0].gridX + 0.5f, 1, nFinalPath[0].gridY + 0.5f), speed * Time.deltaTime);
            }
            else
            {
                nFinalPath.RemoveAt(0);
            }


        }

        
    }

    public void FindPathFunction(GameObject target)
    {
        nFinalPath = new List<Node>();
        this.target = target;
        targetPosition = target.transform.position;
        startPosition = transform.position;
    }
    //IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    public IEnumerator FindPath()
    {
        //Vector3 startPos = transform.position;
        Vector3 targetPos = targetPosition;
        Vector3 startPos = startPosition;
        Node startNode = pathFindingGrid.NodeFromWorldPos(startPos);
        Node targetNode = pathFindingGrid.NodeFromWorldPos(targetPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        //Debug.Log(startNode.position);
        //Debug.Log(targetNode.position);
        openList.Add(startNode);
        while(openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {

                if (openList[i].FCost <= currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    //Debug.Log("looping openlist");
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            ClosedList.Add(currentNode);

            if (currentNode ==  targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach (Node neighborNode in pathFindingGrid.GetNeighboringNodes(currentNode))
            {
                if (neighborNode.isWall || ClosedList.Contains(neighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhatterDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhatterDistance(neighborNode, targetNode);
                    neighborNode.parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        yield return null;
    }

    private int GetManhatterDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

    private void GetFinalPath(Node startNode, Node targetNode)
    {
        Node currentNode = targetNode;
        //nFinalPath = new List<Node>();
        List<Node> path = new List<Node>();
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        nFinalPath = path;
        //nFinalPath.Reverse();

        //pathFindingGrid.finalPath = finalPath;
    }

    
}
