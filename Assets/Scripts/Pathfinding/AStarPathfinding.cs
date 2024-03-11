using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class AStarPathfinding : MonoBehaviour
{
    public DungeonManager DungeonManager;
    private HashSet<Node> _openList = new HashSet<Node>(), _closedList = new HashSet<Node>();
    private Dictionary<Vector3Int, Node> _allNodes = new Dictionary<Vector3Int, Node>();
    public Vector3Int StartPos, GoalPos;
    private Node current; // current position of the enemy
    private List<Node> neighbors;
    private List<Node> sortedOpenList;

    public Stack<Vector3Int> Path { get; private set; }

    private void Initialize()
    {
        current = GetNode(StartPos);

        _openList.Add(current);
        
        neighbors = neighbors ?? new List<Node>();
        neighbors.Clear();
        
        sortedOpenList = sortedOpenList ?? new List<Node>();
        sortedOpenList.Clear();
    }

    public void SetupVariables(Vector3 seekerPos, Vector3Int targetPos)
    {
        current = null;
        StartPos = StartPos = Vector3Int.RoundToInt(seekerPos);
        GoalPos = targetPos;
        Path = null;

        _openList.Clear();
        _closedList.Clear();
    }

    public void PathFinding()
    {
        // main algorithm
        if (current == null)
        {
            Initialize();
        }
        while (_openList.Count > 0 && Path == null)
        {
            neighbors = FindNeighbors(current.Position);
            ExamineNeighbors(neighbors, current);
            UpdateCurrentTile(ref current);
            Path = GeneratePath(current);
        }
    }

    private List<Node> FindNeighbors(Vector3Int parentPos)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    // skipping the parent node
                    Vector3Int neighborPos = new Vector3Int(parentPos.x + x, parentPos.y + y, parentPos.z);

                    bool neighborInDungeon = !(neighborPos.x < 0 || neighborPos.y < 0 ||
                                               neighborPos.x >= DungeonManager.DungeonMap.GetLength(0) ||
                                               neighborPos.y >= DungeonManager.DungeonMap.GetLength(1));

                    if (neighborInDungeon && neighborPos != StartPos &&
                        DungeonManager.DungeonMap[neighborPos.x, neighborPos.y] == 1)
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<Node> targetNeighbors, Node current)
    {
        for (int i = 0; i < targetNeighbors.Count; i++)
        {
            Node neighbor = targetNeighbors[i];

            if (!ConnectedDiagonally(current, neighbor))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbor.Position, current.Position); // calculating G score

            if (_openList.Contains(neighbor))
            {
                if (this.current.G + gScore < neighbor.G)
                {
                    CalculateValues(current, neighbor, gScore);
                }
            }
            else if (!_closedList.Contains(neighbor))
            {
                CalculateValues(current, neighbor, gScore);

                _openList.Add(neighbor);
            }
        }
    }

    private void CalculateValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(neighbor.Position.x - GoalPos.x) + Mathf.Abs(neighbor.Position.y - GoalPos.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    // calculating G score using the nodes
    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Mathf.Abs(x - y) % 2 == 1)
        {
            gScore = 10; // horizontal or vertical
        }
        else
        {
            gScore = 14; // cross direction
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        _openList.Remove(current);
        _closedList.Add(current);

        if (_openList.Count > 0)
        {
            SortByScore();
            this.current = _openList.GetFirst(); // sort the list and find the node with lowest F value
        }
        
        void SortByScore()
        {
            if (_openList.Count > 1)
            {
                sortedOpenList.Clear();
                sortedOpenList.AddRange(_openList);
                sortedOpenList.Sort((x, y) => x.F.CompareTo(y.F));
                _openList.Clear();
                foreach (Node node in sortedOpenList)
                {
                    _openList.Add(node);
                }
            }
        }
    }

    private Node GetNode(Vector3Int position)
    {
        if (_allNodes.TryGetValue(position, out Node node))
        {
            return node;
        }

        Node newNode = new Node(position);
        _allNodes.Add(position, newNode);
        return newNode;
    }

    private bool ConnectedDiagonally(Node current, Node neighbor)
    {
        Vector3Int direction = current.Position - neighbor.Position;

        Vector2Int first = new Vector2Int(current.Position.x + (direction.x * -1), current.Position.y);
        Vector2Int second = new Vector2Int(current.Position.x, current.Position.y + (direction.y * -1));

        return DungeonManager.DungeonMap[first.x, first.y] == 1 && DungeonManager.DungeonMap[second.x, second.y] == 1;
    }

    private Stack<Vector3Int> GeneratePath(Node currentNode)
    {
        if (current.Position == GoalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while (currentNode.Position != StartPos)
            {
                finalPath.Push(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            return finalPath;
        }

        return null;
    }
}