using UnityEngine;

public class GameConfigData : MonoBehaviour
{
    private static GameConfigData _instance;

    public static GameConfigData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ((GameObject)Instantiate(Resources.Load("GameConfigData"))).GetComponent<GameConfigData>();
            }

            return _instance;
        }
    }

    [Header("Dungeon Variables")] public int DungeonPadding;
    public int DungeonRows, DungeonColumns;
    public int MinRoomSize, MaxRoomSize;

    [Header("Dungeon Tiles")] public GameObject[] FloorTiles;
    public GameObject[] BridgeTiles;
    public GameObject[] WaterTiles;
    public GameObject[] WallTiles;
    public GameObject ExitTile;

    [Header("Dungeon Objects")] public GameObject[] LampObjects;
    public GameObject[] Traps;
    public GameObject Key;
    public GameObject[] Weapons; // weapons that can be used by the player
    public GameObject[] ItemChests;

    public enum CollectibleType
    {
        Medkit,
        Shield,
        Snack
    };

    public GameObject Consumable;

    public SoundAudioClip[] Sounds;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
    }
}