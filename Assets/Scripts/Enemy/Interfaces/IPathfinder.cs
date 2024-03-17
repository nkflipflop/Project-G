using Pathfinding;
using UnityEngine;

public interface IPathfinder
{
    AStar AStar { get; set; }
    Transform Target { get; set; }

    void SetupPathfinding(Transform target, int[,] grid);
}