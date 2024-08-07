using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Pathfinding
{
	public class AStar
	{
		private readonly HashSet<Node> openList = new();
		private readonly HashSet<Node> closedList = new();
		private readonly Dictionary<Vector3Int, Node> allNodes = new();
		private Node current;
		private List<Node> neighbors;
		private List<Node> sortedOpenList;
	
		private int[,] grid;
		private Vector3Int startPos;
		private Vector3Int goalPos;

		public Stack<Vector3Int> Path { get; private set; }

		public void Initialize(int[,] grid, Vector3Int startPos, Vector3Int goalPos)
		{
			this.grid = grid;
			this.startPos = startPos;
			this.goalPos = goalPos;
		}

		private void InitiateForPathfinding()
		{
			current = GetNode(startPos);

			openList.Add(current);
		
			neighbors ??= new List<Node>();
			neighbors.Clear();
		
			sortedOpenList ??= new List<Node>();
			sortedOpenList.Clear();
		}

		public void Reset()
		{
			current = null;
			Path = null;
			grid = null;
			
			openList?.Clear();
			closedList?.Clear();
			allNodes?.Clear();
			neighbors?.Clear();
			sortedOpenList?.Clear();
		}

		public void UpdateObjective(Vector3 seekerPos, Vector3Int targetPos)
		{
			current = null;
			startPos = Vector3Int.RoundToInt(seekerPos);
			goalPos = targetPos;
			Path = null;

			openList.Clear();
			closedList.Clear();
		}

		public void FindPath()
		{
			// main algorithm
			if (current == null)
			{
				InitiateForPathfinding();
			}
			while (openList.Count > 0 && Path == null)
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
												   neighborPos.x >= grid.GetLength(0) ||
												   neighborPos.y >= grid.GetLength(1));

						if (neighborInDungeon && neighborPos != startPos && grid[neighborPos.x, neighborPos.y] == 1)
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

				if (openList.Contains(neighbor))
				{
					if (this.current.G + gScore < neighbor.G)
					{
						CalculateValues(current, neighbor, gScore);
					}
				}
				else if (!closedList.Contains(neighbor))
				{
					CalculateValues(current, neighbor, gScore);

					openList.Add(neighbor);
				}
			}
		}

		private void CalculateValues(Node parent, Node neighbor, int cost)
		{
			neighbor.Parent = parent;
			neighbor.G = parent.G + cost;
			neighbor.H = (Mathf.Abs(neighbor.Position.x - goalPos.x) + Mathf.Abs(neighbor.Position.y - goalPos.y)) * 10;
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
			openList.Remove(current);
			closedList.Add(current);

			if (openList.Count > 0)
			{
				SortByScore();
				this.current = openList.GetFirst(); // sort the list and find the node with lowest F value
			}
		
			void SortByScore()
			{
				if (openList.Count > 1)
				{
					sortedOpenList.Clear();
					sortedOpenList.AddRange(openList);
					sortedOpenList.Sort((x, y) => x.F.CompareTo(y.F));
					openList.Clear();
					foreach (Node node in sortedOpenList)
					{
						openList.Add(node);
					}
				}
			}
		}

		private Node GetNode(Vector3Int position)
		{
			if (allNodes.TryGetValue(position, out Node node))
			{
				return node;
			}

			Node newNode = new Node(position);
			allNodes.Add(position, newNode);
			return newNode;
		}

		private bool ConnectedDiagonally(Node current, Node neighbor)
		{
			Vector3Int direction = current.Position - neighbor.Position;

			Vector2Int first = new Vector2Int(current.Position.x + (direction.x * -1), current.Position.y);
			Vector2Int second = new Vector2Int(current.Position.x, current.Position.y + (direction.y * -1));

			return grid[first.x, first.y] == 1 && grid[second.x, second.y] == 1;
		}

		private Stack<Vector3Int> GeneratePath(Node currentNode)
		{
			if (current.Position == goalPos)
			{
				Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

				while (currentNode.Position != startPos)
				{
					finalPath.Push(currentNode.Position);
					currentNode = currentNode.Parent;
				}

				return finalPath;
			}

			return null;
		}
	}
}