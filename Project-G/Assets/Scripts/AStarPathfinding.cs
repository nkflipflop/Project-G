using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {
	private static AStarPathfinding _instance;

	public static AStarPathfinding MyInstance {
		get {
			if (_instance == null)
				_instance = FindObjectOfType<AStarPathfinding>();

			return _instance;
		}
	}

	public DungeonManager DungeonManager;
	private HashSet<Node> _openList, _closedList;
	private Stack<Vector3Int> _path;
	private Dictionary<Vector3Int, Node> _allNodes = new Dictionary<Vector3Int, Node>();
	public Vector3Int StartPos, GoalPos;
	private Node _current;      // current position of the enemy

	public Vector3Int Current { get { return _current.Position; } }
	public Stack<Vector3Int> Path { get { return _path; } }

	private void Initialize() {
		_current = GetNode(StartPos);

		_openList = new HashSet<Node>();
		_closedList = new HashSet<Node>();
		
		_openList.Add(_current);
	}

	public void PathFinding() {		// main algorithm
		if (_current == null)
			Initialize();
		Debug.Log("StartPos: " + StartPos);
		Debug.Log("Goal: " + GoalPos);
		while (_openList.Count > 0 && _path == null) {
			Debug.Log("Current: " + _current.Position);
			List<Node> neighbors = FindNeighbors(_current.Position);

			ExamineNeighbors(neighbors, _current);

			UpdateCurrentTile(ref _current);

			_path = GeneratePath(_current);
		}
		foreach (Vector3Int pos in _path)
            Debug.Log("In stack: " + pos);
	}

	private List<Node> FindNeighbors(Vector3Int parentPos) {
		List<Node> neighbors = new List<Node>();
		//Debug.Log("----------------------");
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x != 0 || y != 0) {		// skipping the parent node
					Vector3Int neighborPos = new Vector3Int(parentPos.x + x, parentPos.y + y, parentPos.z);
					
					if (neighborPos != StartPos && DungeonManager.DungeonMap[neighborPos.x, neighborPos.y] == 1) {
						//Debug.Log(neighborPos);
						Node neighbor = GetNode(neighborPos);
						neighbors.Add(neighbor);
					}
				}
			}
		}
		//Debug.Log("----------------------");

		return neighbors;
	}

	private void ExamineNeighbors(List<Node> neighbors, Node current) {
		Debug.Log("ExamineNeighbors Func");
		for (int i = 0; i < neighbors.Count; i++) {
			Node neighbor = neighbors[i];

			if (!ConnectedDiagonally(current, neighbor))
				continue;

			int gScore = DetermineGScore(neighbors[i].Position, current.Position);		// calculating G score

			if (_openList.Contains(neighbor)) {
				if (_current.G + gScore < neighbor.G) {
					CalculateValues(current, neighbor, gScore);
				}
			}
			else if (!_closedList.Contains(neighbor)) {
				CalculateValues(current, neighbor, gScore);

				_openList.Add(neighbor);
			}
			//foreach ( Node node in _closedList)
				//Debug.Log("Closed List: " + node.Position);
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
			gScore = 10;
		else
			gScore = 14;
		
		return gScore;
	}

	private void UpdateCurrentTile(ref Node current) {
		Debug.Log("UpdateCurrentTile Func");
		_openList.Remove(current);
		_closedList.Add(current);

		if (_openList.Count > 0) {
			_current = _openList.OrderBy(x => x.F).First();		// sort the list and find the node with lowest F value
		}
		Debug.Log("Updated current: " + _current.Position);

		// foreach ( Node node in _openList)
		// 	Debug.Log("Open List: " + node.Position);
		// foreach ( Node node in _closedList)
		// 	Debug.Log("Closed List: " + node.Position);
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

		if (DungeonManager.DungeonMap[first.x, first.y] == 0 || DungeonManager.DungeonMap[second.x, second.y] == 0) {
			return false;
		}

		return true;
	}

	private Stack<Vector3Int> GeneratePath(Node current) {
		if (_current.Position == GoalPos) {
			Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

			while (_current.Position != StartPos) {
				finalPath.Push(current.Position);
				_current = _current.Parent;
			}

			return finalPath;
		}
		return null;
	}
}
