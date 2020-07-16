using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public DungeonManager DungeonManager;
	public PlayerManager PlayerManager;

	private int _dungeonLevel = 0;
	public int DungeonLevel { get{ return _dungeonLevel; } }

	private void Start() {
		LoadLevelData();
		System.DateTime start = System.DateTime.Now;
		LoadDungeon();
		System.DateTime end = System.DateTime.Now;
		Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
	}

	private void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
			TogglePause();
		}
	}

	// Pass to the next level
	public void LoadNextLevel() {
		SaveLevelData();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void LoadDungeon() {
		DungeonManager.CreateDungeon(this);
		DungeonManager.SpawnEverything(_dungeonLevel);
	}

	/// <summary> Gets the level data from DataManager and makes the assignments </summary>
	private void LoadLevelData() {
		_dungeonLevel = DataManager.Instance.DungeonLevel;
		PlayerManager.LoadPlayerData();									// loading player's data
	}

	/// <summary> Gets the level data and saves it in DataManager at the end of the level </summary>
	private void SaveLevelData() {
		DataManager.Instance.DungeonLevel++;
		if (DataManager.Instance.DungeonLevel == 7)      // after level 7, reset the game
			DataManager.Instance.DungeonLevel = 0;
		PlayerManager.SavePlayerData();	
	}

	/// <summary> Pauses/Resumes the game by toggling the current situation </summary>
	private void TogglePause() {
		Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
	}
}
