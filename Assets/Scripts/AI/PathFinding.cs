using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

using Generation.Terrain;
using System.Linq;

namespace Ai.PathFinding
{
    public class PathFinding : MonoBehaviour
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAl_COST = 14;

        public Transform target;
        public float speed;
        private Vector3 targetPos;
        private void Start()
        {
        }
        List<int2> pathList = new List<int2>();
        private void Update()
        {
            if (target != null && Vector3.Distance(targetPos, target.position) > 0.5f)
            {
                NativeList<int2> nativePath = new NativeList<int2>(Allocator.TempJob);
                var findPathJob = new FindPathJob
                {
                    endPosition = PathFindingNodeFromWorldPosition(target.position),
                    startPosition = PathFindingNodeFromWorldPosition(transform.position),
                    finalPath = nativePath,
                };
                var findPathHandle = findPathJob.Schedule();
                findPathHandle.Complete();
                pathList = nativePath.ToArray().ToList();
                nativePath.Dispose();
                pathList.Reverse();
            }
            if (pathList != null && pathList.Count > 0)
            {
                int2 pos = PathFindingNodeFromWorldPosition(transform.position);
                if (!pos.Equals(pathList[0]))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(pathList[0].x + 0.5f, 1, pathList[0].y + 0.5f), speed * Time.deltaTime);
                }
                else
                {
                    Debug.Log("YEs");
                    pathList.RemoveAt(0);
                }
            }
            targetPos = target.position;
        }

        public int2 PathFindingNodeFromWorldPosition(Vector3 worldPos)
        {
            int2 pos = new int2(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.z));
            return pos;
        }

        [BurstCompile]
        private struct FindPathJob : IJob
        {
            public int2 startPosition;
            public int2 endPosition;
            public NativeList<int2> finalPath;
            public void Execute()
            {
                //int2 gridSize = new int2(WorldMain.chunkGridSizeX * WorldMain.chunkSizeX, WorldMain.chunkGridSizeY * WorldMain.chunkSizeY);
                int2 gridSize = new int2(16 * 3, 16 * 3);
                NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        PathNode pathNode = new PathNode();
                        pathNode.x = x;
                        pathNode.y = y;

                        pathNode.index = CalculateIndex(x, y, gridSize.x);

                        pathNode.gCost = int.MaxValue;
                        pathNode.hCost = CalculateDistanceCost(new int2(x, y), endPosition);
                        pathNode.CalculateFCost();

                        pathNode.isWalkable = true;
                        pathNode.cameFromNodeIndex = -1;

                        pathNodeArray[pathNode.index] = pathNode;
                    }
                }

                NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                neighbourOffsetArray[0] = new int2(-1, 0); // Left
                neighbourOffsetArray[1] = new int2(+1, 0); // Right
                neighbourOffsetArray[2] = new int2(0, +1); // Up
                neighbourOffsetArray[3] = new int2(0, -1); // Down
                neighbourOffsetArray[4] = new int2(-1, -1); // Left Down
                neighbourOffsetArray[5] = new int2(-1, +1); // Left Up
                neighbourOffsetArray[6] = new int2(+1, -1); // Right Down
                neighbourOffsetArray[7] = new int2(+1, +1); // Right Up
            
            
            

                int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

                PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
                startNode.gCost = 0;
                startNode.CalculateFCost();
                pathNodeArray[startNode.index] = startNode;

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

                openList.Add(startNode.index);
                while (openList.Length > 0)
                {
                    int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                    PathNode currentNode = pathNodeArray[currentNodeIndex];
                    // If we found a path
                    if (currentNodeIndex == endNodeIndex)
                    {
                        break;
                    }
                    // Remove current node from open list
                    for (int i = 0; i < openList.Length; i++)
                    {
                        if (openList[i] == currentNodeIndex)
                        {
                            openList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    closedList.Add(currentNodeIndex);

                    for (int i = 0; i < neighbourOffsetArray.Length; i++)
                    {
                        int2 neighbourOffset = neighbourOffsetArray[i];
                        int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);

                        if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                        {
                            continue;
                        }

                        int neighbourIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                        if (closedList.Contains(neighbourIndex))
                        {
                            // Already searched this node
                            continue;
                        }

                        PathNode neighbourNode = pathNodeArray[neighbourIndex];
                        if (!neighbourNode.isWalkable)
                        {
                            continue;
                        }


                        int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                        int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                        if (tentativeGCost < neighbourNode.gCost)
                        {
                            neighbourNode.cameFromNodeIndex = currentNodeIndex;
                            neighbourNode.gCost = tentativeGCost;
                            neighbourNode.CalculateFCost();
                            pathNodeArray[neighbourIndex] = neighbourNode;

                            if (!openList.Contains(neighbourNode.index))
                            {
                                openList.Add(neighbourNode.index);
                            }
                        }

                    }

                }

                PathNode endNode = pathNodeArray[endNodeIndex];
                if (endNode.cameFromNodeIndex == -1)
                {
                    // Did not find a path;
                }
                else
                {
                    // Found a path
                    //finalPath = CalculatePath(pathNodeArray, endNode);
                    CalculatePath(pathNodeArray, endNode);
                }


                pathNodeArray.Dispose();
                openList.Dispose();
                closedList.Dispose();
                neighbourOffsetArray.Dispose();
            }

            //private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
            private void CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
            {
                if (endNode.cameFromNodeIndex == -1)
                {
                    //return new NativeList<int2>(Allocator.Temp);
                    finalPath = new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    // Found a path
                    //NativeList<int2> path = new NativeList<int2>(Allocator.Temp);

                    PathNode currentNode = endNode;
                    while (currentNode.cameFromNodeIndex != -1)
                    {
                        PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                        //path.Add(new int2(cameFromNode.x, cameFromNode.y));
                        finalPath.Add(new int2(cameFromNode.x, cameFromNode.y));
                        currentNode = cameFromNode;
                    }
                    //return path;
                }
            }

            private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
            {
                return
                    gridPosition.x >= 0 &&
                    gridPosition.y >= 0 &&
                    gridPosition.x < gridSize.x &&
                    gridPosition.y < gridSize.y;
            }
            private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
            {
                PathNode lowestCostPathNode = pathNodeArray[openList[0]];

                for (int i = 1; i < openList.Length; i++)
                {
                    PathNode testPathNode = pathNodeArray[openList[i]];
                    if (testPathNode.fCost < lowestCostPathNode.fCost)
                    {
                        lowestCostPathNode = testPathNode;
                    }
                }

                return lowestCostPathNode.index;
            }
            public static int CalculateDistanceCost(int2 aPosition, int2 bPosition)
            {
                int xDistance = math.abs(aPosition.x - bPosition.x);
                int yDistance = math.abs(aPosition.y - bPosition.y);
                int remaining = math.abs(xDistance - yDistance);
                return MOVE_DIAGONAl_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;

            }
            public static int CalculateIndex(int x, int y, int gridWidth)
            {
                return x + y * gridWidth;
            }
            public struct PathNode
            {
                public int x;
                public int y;

                public int index;

                public int gCost; // Move cost from start node to this node
                public int hCost; // Estimated cost from this node to end node
                public int fCost; // gCost + hCost;

                public bool isWalkable;

                public int cameFromNodeIndex;

                public void CalculateFCost()
                {
                    fCost = gCost + hCost;
                }
            }
        }
    }

}
