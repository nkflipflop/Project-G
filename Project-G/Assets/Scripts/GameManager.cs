using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DungeonManager DungeonManager;

    private int _dungeonLevel = 0;

    private void Awake() {
		LoadLevelData();
        System.DateTime start = System.DateTime.Now;
        LoadDungeon();
        System.DateTime end = System.DateTime.Now;
		Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
			SaveLevelData();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
    }

    private void LoadDungeon() {
        DungeonManager.CreateDungeon();
        DungeonManager.SpawnEverything(_dungeonLevel);
    }

	private void LoadLevelData() {
		_dungeonLevel = DataManager.DungeonLevel;
	}

	private void SaveLevelData() {
		DataManager.DungeonLevel++;
        if (DataManager.DungeonLevel == 5)      // after level 5, resets the game
        	DataManager.DungeonLevel = 0;
	}
}
