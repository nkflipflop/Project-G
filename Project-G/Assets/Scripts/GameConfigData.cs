using UnityEngine;

public class GameConfigData : MonoBehaviour {
	private static GameConfigData _instance;
	public static GameConfigData Instance {
		get {
			if (_instance == null)
				_instance = (Instantiate(Resources.Load("GameConfigData")) as GameObject).GetComponent<GameConfigData>();
			return _instance;
		}
	}
    // DungeonMap variables
    public int DungeonRows, DungeonColumns;
	public int DungeonPadding;
	public int MinRoomSize, MaxRoomSize;

    // Prefabs that are used in the dungeon
    public GameObject[] FloorTiles;
	public GameObject[] BridgeTiles;
	public GameObject[] WaterTiles;
	public GameObject[] WallTiles;
	public GameObject ExitTile;
	public GameObject[] LampObjects;
	// public GameObject[] Enemies;
	public GameObject[] Traps;
	public GameObject Key;
    public GameObject[] Weapons;        // weapons that can be used by the player
	
	public SoundAudioClip[] Sounds;
	public class SoundAudioClip {
		public SoundManager.Sound sound;
		public AudioClip audioClip;
	}
}
