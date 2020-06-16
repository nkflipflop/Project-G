using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DungeonManager DungeonManager;

    private int _dungeonLevel = 0;

    private void Awake() {
        DungeonManager.CreateDungeon();
        DungeonManager.RandomEnemySpawner(_dungeonLevel);
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
