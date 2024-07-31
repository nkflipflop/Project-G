using General;
using UnityEngine;
using Utilities;

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
        Log.Debug(("Dungeon Loading Time:", end.Subtract(start).Milliseconds, "ms"), color: Color.green);
        Signals.Common.Signal.SetInputState?.Invoke(true);
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiController.TogglePause();
            }

            if ((playerManager as IHealthInteractable).IsDead)
            {
                isGameOver = true;
                _ = uiController.ActivateGameOverScreen();
            }
        }
    }

    // Go to the next level
    public void LoadNextLevel()
    {
        SaveLevelData();
        _ = LoadingManager.instance.LoadScene(Constants.DUNGEON);
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