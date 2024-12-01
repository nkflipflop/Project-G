using Controllers;
using Cysharp.Threading.Tasks;
using Gameplay.Runtime.Dungeon;
using General;
using Signals.Common;
using UnityEngine;
using Utilities;

namespace Gameplay.Runtime.Controllers
{
	public class GameManager : Singleton<GameManager>
	{
		private GameStatus gameStatus;
		public GameStatus GameStatus => gameStatus;

		#region Camera
		private CameraControllerBase cameraController;
		public Camera MainCamera => cameraController?.cam;
		#endregion

		[field: SerializeField] public Player.Player Player { get; set; }

		private DungeonController dungeonController;
		public DungeonController DungeonController => dungeonController ??= new DungeonController();

		private void OnEnable()
		{
			Signal.OnSceneLoadStepCompleted += OnSceneLoadStepCompleted;
		}

		private void OnDisable()
		{
			Signal.OnSceneLoadStepCompleted -= OnSceneLoadStepCompleted;
		}
		
		private void OnSceneLoadStepCompleted(int sceneIndex, SceneLoader.LoadingStep step)
		{
			if (sceneIndex == Constants.DUNGEON_SCENE_INDEX && step == SceneLoader.LoadingStep.LoadingOperationCompleted)
			{
				InitializeLevel();
			}
		}

		private void InitializeLevel()
		{
			SetGameStatus(GameStatus.Starting);
			LoadLevelData();
			DungeonController.CreateDungeon();
			DungeonController.SpawnEverything(0);
			Signals.Gameplay.Signal.OnLevelStarted?.Invoke();
			StartGameplay();
		}
        
		public void StartGameplay()
		{
			Signal.SetInputState?.Invoke(true);
			SetGameStatus(GameStatus.Playing);
		}
		
		public void LoadNextLevel()
		{
			SaveLevelData();
			_ = SceneLoader.instance.LoadScene(Constants.DUNGEON_SCENE_INDEX);
		}
		
		#region Assignments
		
		public void SetCameraController(CameraControllerBase cameraController)
		{
			this.cameraController = cameraController;
		}
		
		#endregion
		
		#region Game Status Methods
		
		public void SetGameStatus(GameStatus status)
		{
			GameStatus cachedStatus = gameStatus;
        
			switch (status)
			{
				case GameStatus.Success:
				case GameStatus.Fail:
					gameStatus = GameStatus.Finished | status;
					break;
				default:
					gameStatus = status;
					break;
			}
        
			if (cachedStatus != gameStatus)
			{
				Signals.Gameplay.Signal.OnGameStatusChanged?.Invoke(gameStatus);
			}
		}

		public void RemoveFromGameStatus(GameStatus status)
		{
			GameStatus cachedStatus = gameStatus;
        
			gameStatus &= ~status;
        
			if (cachedStatus != gameStatus)
			{
				Signals.Gameplay.Signal.OnGameStatusChanged?.Invoke(gameStatus);
			}
		}
		
		#endregion
		
		#region Pause & Resume
		
		public void Pause()
		{
			if (gameStatus.Contains(GameStatus.Playing))
			{
				SetGameStatus(GameStatus.Pause);
				// CommandManager.PushCommandInMainFlow(new PanelCommand(PanelInitializer.instance.GetPanel<PausePanel>()), prepend: true);
			}
		}
		public void Resume()
		{
			SetGameStatus(GameStatus.Playing);
		}
		
		#endregion

		#region Save & Load

		/// <summary> Gets the level data from DataManager and makes the assignments </summary>
		private void LoadLevelData()
		{
			// dungeonLevel = DataManager.instance.DungeonLevel;
			Player.LoadPlayerData();		// loading player's data
		}

		/// <summary> Gets the level data and saves it in DataManager at the end of the level </summary>
		private void SaveLevelData()
		{
			DataManager.instance.DungeonLevel++;
			Player.SavePlayerData();
		}
		
		/// <summary> Resets everything in DataManager </summary>
		public void ResetLevelData()
		{
			DataManager.instance.ResetData();
		}

		#endregion

		public void OnPlayerDied()
		{
			SetGameStatus(GameStatus.Fail);
			// TODO: display fail screen
		}
	}
}