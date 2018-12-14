using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform startPosition;
    public Transform targetPosition;
    public GameObject pathfindingGrid;

    public float speed = 4.0f;

    Grid grid;
    Node targetNode;

    Vector3 goToPosition;

    private void Awake()
    {
        grid = pathfindingGrid.GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(startPosition.position, targetPosition.position);

        if (grid.path != null && startPosition.position != targetPosition.position && grid.path.Count > 0)
        {
            targetNode = grid.path[0];
            goToPosition = new Vector3(targetNode.position.x, startPosition.position.y, targetNode.position.z);
        }
        else if (startPosition.position == targetPosition.position)
        {
            grid.path = null;
            targetNode = null;
        }
        startPosition.position = Vector3.MoveTowards(startPosition.position, goToPosition, speed * Time.deltaTime);
        Vector3 targetDir = goToPosition - startPosition.position;
        Vector3 newDir = Vector3.RotateTowards(startPosition.forward, targetDir, speed * Time.deltaTime, 0.0f);
        startPosition.rotation = Quaternion.LookRotation(newDir);

    }

    void FindPath(Vector3 _startPosition, Vector3 _targetPosition)
    {
        Node startNode = grid.NodeFromWorldPosition(_startPosition);
        Node targetNode = grid.NodeFromWorldPosition(_targetPosition);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0 && !closedList.Contains(targetNode))
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetPath(startNode, targetNode);
            }

            foreach (Node neighborNode in grid.GetNeighborNodes(currentNode))
            {
                if (!neighborNode.walkable || closedList.Contains(neighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode);
                    neighborNode.parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
    }

    void GetPath(Node _startingNode, Node _endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = _endNode;

        while(currentNode != _startingNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetManhattenDistance(Node _nodeA, Node _nodeB)
    {
        int x = Mathf.Abs(_nodeA.gridX - _nodeB.gridX);
        int y = Mathf.Abs(_nodeA.gridY - _nodeB.gridY);

        return x + y;
    }
}
