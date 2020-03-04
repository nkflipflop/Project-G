using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    private AStarPathfinding _aStar;
    [SerializeField] private float _speed = 1f;

    private Vector3 _targetPos;
    private Vector3 _distanceBtwPlayer;
    private int _maxPathLength = 10;

    // Start is called before the first frame update
    void Start() {
        _aStar = gameObject.GetComponent<AStarPathfinding>();
        _aStar.StartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        _aStar.GoalPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y, (int)Player.transform.position.z);
        _targetPos = new Vector3Int(1000, 0, 0);        // null value

        InvokeRepeating("CheckPlayerPosition", 5.0f, 0.5f);     // run this function every 0.5 sec
    }

    // Update is called once per frame
    void FixedUpdate() {
        _aStar.PathFinding();
        FollowPath();
    }

    private void FollowPath() {
        _distanceBtwPlayer = Player.transform.position - transform.position;
        if (Mathf.Abs(_distanceBtwPlayer.x) < 0.6f || Mathf.Abs(_distanceBtwPlayer.y) < 0.6f) {
            _targetPos = Player.transform.position;
        }
        else if (_aStar.Path != null && _aStar.Path.Count > 0 && _aStar.Path.Count < _maxPathLength) {
            if (_targetPos == new Vector3Int(1000, 0, 0) || transform.position == _targetPos) {
                _targetPos = _aStar.Path.Pop();
            }
        }
        else
            _targetPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);       // if not chasing the player, stay where you are

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _speed);      // moving the enemy towards to target
    }

    private void CheckPlayerPosition() {
        Vector3Int playerPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y, (int)Player.transform.position.z);
        if (_aStar.GoalPos != playerPos) {
            _aStar.Current = null;
            _aStar.StartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            _aStar.GoalPos = new Vector3Int((int)Player.transform.position.x, (int)Player.transform.position.y, (int)Player.transform.position.z);
            _aStar.Path = null;
        }
    }
}
