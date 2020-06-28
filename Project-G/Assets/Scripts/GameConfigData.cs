using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config")]
public class GameConfigData : ScriptableObject {
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
}
