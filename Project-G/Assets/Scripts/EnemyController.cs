using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    private Vector3 smoothVelocity = Vector3.zero;
    public float smoothTime = 1.0f;
    // Start is called before the first frame update
    void Start() {
        AStarPathfinding.MyInstance.StartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    // Update is called once per frame
    void Update() {
        AStarPathfinding.MyInstance.PathFinding();
        //AStarPathfinding.MyInstance.GoalPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y, (int)Player.transform.position.z);
        //transform.position = Vector3.SmoothDamp(transform.position, AStarPathfinding.MyInstance.Current, ref smoothVelocity, smoothTime);
        // if (AStarPathfinding.MyInstance.Path != null) {
        //     Debug.Log("--------------");
        //     Debug.Log(AStarPathfinding.MyInstance.Path.Count);
        //     foreach (Vector3Int pos in AStarPathfinding.MyInstance.Path)
        //         Debug.Log(pos);
        //     Debug.Log("--------------");
        // }
    }
}
