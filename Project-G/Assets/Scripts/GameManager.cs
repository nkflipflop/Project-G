using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DungeonManager DungeonManager;
	public GameObject Player;

    private int _dungeonLevel = 0;

    private void Awake() {
		LoadLevelData();
        System.DateTime start = System.DateTime.Now;
        LoadDungeon();
        System.DateTime end = System.DateTime.Now;
		Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {		// press "space" to skip the current level
			SaveLevelData();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
    }

    private void LoadDungeon() {
        DungeonManager.CreateDungeon();
        DungeonManager.SpawnEverything(_dungeonLevel);
    }

	/// <summary> Gets the level data from DataManager and makes the assignments </summary>
	private void LoadLevelData() {
		_dungeonLevel = DataManager.Instance.DungeonLevel;
		Player.GetComponent<DamageHelper>().Health = DataManager.Instance.Health;													// loading player's health
	}

	/// <summary> Gets the level data and saves it in DataManager at the end of the level </summary>
	private void SaveLevelData() {
		DataManager.Instance.DungeonLevel++;
        if (DataManager.Instance.DungeonLevel == 5)      // after level 5, reset the game
        	DataManager.Instance.DungeonLevel = 0;
		DataManager.Instance.Health = Player.GetComponent<DamageHelper>().Health;		// storing player's health
		DataManager.Instance.WeaponID = Player.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<WeaponPrefab>().ID;			// storing player's weapon
	}
}
