using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public DungeonManager DungeonManager;
	public PlayerManager PlayerManager;
	public GameObject PauseMenu = null;
	public GameObject GameOverScreen = null;

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
				TogglePause();
			}

			if (PlayerManager.HealthController.IsDead) {
				StartCoroutine(ActivateGameOverScreen());
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
		if (DataManager.Instance.DungeonLevel == 7)      // after level 7, reset the game
			ResetLevelData();
	}

	/// <summary> Resets everything in DataManager </summary>
	public void ResetLevelData() {
		DataManager.Instance.ResetData();
	}

	/// <summary> Pauses/Resumes the game by toggling the current situation </summary>
	public void TogglePause() {
		Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
		PauseMenu.SetActive(Time.timeScale != 1);		// activate/deactivate the Pause Menu
	}

	private IEnumerator ActivateGameOverScreen() {
		yield return new WaitForSeconds(1f);		// 1 sec delay
		_isGameOver = true;
		GameOverScreen.SetActive(true);
	}
}
