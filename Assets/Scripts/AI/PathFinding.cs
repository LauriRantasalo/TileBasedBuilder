using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    GameObject main;
    PathFindingGrid pathFindingGrid;
    [HideInInspector]
    public GameObject target;
    Vector3 targetPos = new Vector3();
    //public Transform startPosition;
    //public Transform targetPosition;

    public float speed = 5f;
    private void Awake()
    {
        main = GameObject.Find("Main");
        pathFindingGrid = main.GetComponent<PathFindingGrid>();
    }

    void Update()
    {
        /*
        Node charNode = pathFindingGrid.NodeFromWorldPos(transform.position);
        Vector2 charGridPosition = new Vector2(charNode.gridX, charNode.gridY);
        */
        if (target != null && target.transform.position != targetPos)
        {
            targetPos = target.transform.position;
            FindPath();
            //Debug.Log("Finding target");
        }

        if (finalPath != null )
        {
            if (pathFindingGrid.NodeFromWorldPos(transform.position) != finalPath[0])
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(finalPath[0].gridX, 1, finalPath[0].gridY), speed * Time.deltaTime);
            }
            else
            {
                finalPath.RemoveAt(0);
            }

            
        }
        
        //grid.CreateGrid();
        //FindPath(startPosition.position, targetPosition.position);
    }

    public void FindPath()
    {
        Vector3 startPos = transform.position;

        Node startNode = pathFindingGrid.NodeFromWorldPos(startPos);
        Node targetNode = pathFindingGrid.NodeFromWorldPos(targetPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        openList.Add(startNode);
        while(openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost <= currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
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
    }

    private int GetManhatterDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

    List<Node> finalPath = new List<Node>();
    private void GetFinalPath(Node startNode, Node targetNode)
    {
        Node currentNode = targetNode;
        finalPath = new List<Node>();
        while(currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        finalPath.Reverse();
        
        //pathFindingGrid.finalPath = finalPath;
    }

    /*
     * 
     * private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(pathFindingGrid.gridSizeX, 1, pathFindingGrid.gridSizeY));

        if (pathFindingGrid.grid != null)
        {
            foreach (Node node in pathFindingGrid.grid)
            {
                if (finalPath != null)
                {
                    if (finalPath.Contains(node))
                    {
                        Gizmos.DrawCube(new Vector3(node.gridX + 0.5f, 1, node.gridY + 0.5f), Vector3.one);
                        Gizmos.color = Color.yellow;
                    }
                }

            }
        }
    }
     * */
}
