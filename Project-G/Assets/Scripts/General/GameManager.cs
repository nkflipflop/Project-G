using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public Camera GameCamera;
	public DungeonManager DungeonManager;
	public PlayerManager PlayerManager;
	public UIController UIController;

	private int _dungeonLevel = 0;
	private bool _isGameOver = false;
	public int DungeonLevel { get{ return _dungeonLevel; } }


	private void Start() {
		SoundManager.Initialize();
		LoadLevelData();
		System.DateTime start = System.DateTime.Now;
		LoadDungeon();
		System.DateTime end = System.DateTime.Now;
		Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
	}

	private void Update() {
		if (!_isGameOver) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				UIController.TogglePause();
			}

			if (PlayerManager.HealthController.IsDead) {
				StartCoroutine(UIController.ActivateGameOverScreen());
				_isGameOver = true;
			}
		}
	}

	// Go to the next level
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
		PlayerManager.SavePlayerData();
	}

	/// <summary> Resets everything in DataManager </summary>
	public void ResetLevelData() {
		DataManager.Instance.ResetData();
	}
}
