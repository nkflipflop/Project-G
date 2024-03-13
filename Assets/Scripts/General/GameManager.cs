using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private DungeonManager dungeonManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private UIController uiController;

    private int dungeonLevel;
    private bool isGameOver;
    public int DungeonLevel => dungeonLevel;

    private void Start()
    {
        SoundManager.Initialize();
        LoadLevelData();
        System.DateTime start = System.DateTime.Now;
        LoadDungeon();
        System.DateTime end = System.DateTime.Now;
        Debug.Log("Dungeon Loading Time: " + end.Subtract(start).Milliseconds + "ms");
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiController.TogglePause();
            }

            if (playerManager.HealthController.IsDead)
            {
                isGameOver = true;
                uiController.ActivateGameOverScreen();
            }
        }
    }

    // Go to the next level
    public void LoadNextLevel()
    {
        SaveLevelData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadDungeon()
    {
        dungeonManager.CreateDungeon(this);
        dungeonManager.SpawnEverything(dungeonLevel);
    }

    /// <summary> Gets the level data from DataManager and makes the assignments </summary>
    private void LoadLevelData()
    {
        dungeonLevel = DataManager.instance.DungeonLevel;
        playerManager.LoadPlayerData(); // loading player's data
    }

    /// <summary> Gets the level data and saves it in DataManager at the end of the level </summary>
    private void SaveLevelData()
    {
        DataManager.instance.DungeonLevel++;
        playerManager.SavePlayerData();
    }

    /// <summary> Resets everything in DataManager </summary>
    public void ResetLevelData()
    {
        DataManager.instance.ResetData();
    }
}