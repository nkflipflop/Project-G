using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {

	public DungeonManager DungeonManager;
	private HashSet<Node> _openList, _closedList;
	private Stack<Vector3Int> _path;
	private Dictionary<Vector3Int, Node> _allNodes = new Dictionary<Vector3Int, Node>();
	public Vector3Int StartPos, GoalPos;
	public Node Current;      // current position of the enemy
	public Stack<Vector3Int> Path { 
		get { return _path; } 
		set { _path = value; } 
	}

	private void Initialize() {
		Current = GetNode(StartPos);

		_openList = new HashSet<Node>();
		_closedList = new HashSet<Node>();
		
		_openList.Add(Current);
	}

	public void PathFinding() {		// main algorithm
		if (Current == null)
			Initialize();
		while (_openList.Count > 0 && _path == null) {
			List<Node> neighbors = FindNeighbors(Current.Position);

			ExamineNeighbors(neighbors, Current);

			UpdateCurrentTile(ref Current);

			_path = GeneratePath(Current);
		}
	}

	private List<Node> FindNeighbors(Vector3Int parentPos) {
		List<Node> neighbors = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x != 0 || y != 0) {		// skipping the parent node
					Vector3Int neighborPos = new Vector3Int(parentPos.x + x, parentPos.y + y, parentPos.z);
					
					if (neighborPos != StartPos && DungeonManager.DungeonMap[neighborPos.x, neighborPos.y] == 1) {
						Node neighbor = GetNode(neighborPos);
						neighbors.Add(neighbor);
					}
				}
			}
		}

		return neighbors;
	}

	private void ExamineNeighbors(List<Node> neighbors, Node current) {
		for (int i = 0; i < neighbors.Count; i++) {
			Node neighbor = neighbors[i];

			if (!ConnectedDiagonally(current, neighbor))
				continue;

			int gScore = DetermineGScore(neighbors[i].Position, current.Position);		// calculating G score

			if (_openList.Contains(neighbor)) {
				if (Current.G + gScore < neighbor.G) {
					CalculateValues(current, neighbor, gScore);
				}
			}
			else if (!_closedList.Contains(neighbor)) {
				CalculateValues(current, neighbor, gScore);

				_openList.Add(neighbor);
			}
		}
	}

	private void CalculateValues(Node parent, Node neighbor, int cost) {
		neighbor.Parent = parent;

		neighbor.G = parent.G + cost;

		neighbor.H = (Mathf.Abs(neighbor.Position.x - GoalPos.x) + Mathf.Abs(neighbor.Position.y - GoalPos.y)) * 10;

		neighbor.F = neighbor.G + neighbor.H;
	}

	// calculating G score using the nodes
	private int DetermineGScore(Vector3Int neighbor, Vector3Int current) {
		int gScore = 0;

		int x = current.x - neighbor.x;
		int y = current.y - neighbor.y;

		if (Mathf.Abs(x - y) % 2 == 1)
			gScore = 10;		// horizontal or vertical
		else
			gScore = 14;		// cross direction
		
		return gScore;
	}

	private void UpdateCurrentTile(ref Node current) {
		_openList.Remove(current);
		_closedList.Add(current);

		if (_openList.Count > 0)
			Current = _openList.OrderBy(x => x.F).First();		// sort the list and find the node with lowest F value
	}

	private Node GetNode(Vector3Int position) {
		if (_allNodes.ContainsKey(position))
			return _allNodes[position];
		else {
			Node node = new Node(position);
			_allNodes.Add(position, node);
			return node;
		}
	}

	private bool ConnectedDiagonally(Node current, Node neighbor) {
		Vector3Int direction = current.Position - neighbor.Position;

		Vector2Int first = new Vector2Int(current.Position.x + (direction.x * -1), current.Position.y);
		Vector2Int second = new Vector2Int(current.Position.x, current.Position.y + (direction.y * -1));

		if (DungeonManager.DungeonMap[first.x, first.y] != 1 || DungeonManager.DungeonMap[second.x, second.y] != 1)
			return false;

		return true;
	}

	private Stack<Vector3Int> GeneratePath(Node current) {
		if (Current.Position == GoalPos) {
			Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

			while (current.Position != StartPos) {
				finalPath.Push(current.Position);
				current = current.Parent;
			}

			return finalPath;
		}
		return null;
	}
}
