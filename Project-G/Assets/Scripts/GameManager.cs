using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DungeonManager DungeonManager;
	public DataManager DataManager;
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
		Player.GetComponent<DamageHelper>().Health = DataManager.Health;		// loading player's health
		GameObject weapon = Instantiate(DataManager.Weapon) as GameObject;
		Player.transform.GetChild(0).GetComponent<PlayerHandController>().EquipWeapon(weapon.GetComponent<WeaponBase>());
	}

	private void SaveLevelData() {
		DataManager.DungeonLevel++;
        if (DataManager.DungeonLevel == 5)      // after level 5, reset the game
        	DataManager.DungeonLevel = 0;
		DataManager.Health = Player.GetComponent<DamageHelper>().Health;		// storing player's health
		DataManager.Weapon = Player.transform.GetChild(0).transform.GetChild(0).gameObject;			// storing player's weapon
	}
}
