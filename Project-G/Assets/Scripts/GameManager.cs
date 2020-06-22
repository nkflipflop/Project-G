using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonManager DungeonManager;

    private int _dungeonLevel = 1;

    private void Awake() {
        System.DateTime start = System.DateTime.Now;
        LoadDungeon();
        System.DateTime end = System.DateTime.Now;
		Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void LoadDungeon() {
        DungeonManager.CreateDungeon();
        DungeonManager.PlayerSpawner();
        DungeonManager.RandomEnemySpawner(_dungeonLevel);
        DungeonManager.RandomTrapSpawner(_dungeonLevel);
    }
}
