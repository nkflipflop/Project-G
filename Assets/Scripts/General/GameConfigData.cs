using UnityEngine;

public class GameConfigData : MonoBehaviour
{
    private static GameConfigData instance;

    public static GameConfigData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ((GameObject)Instantiate(Resources.Load("GameConfigData"))).GetComponent<GameConfigData>();
            }

            return instance;
        }
    }

    [Header("Dungeon Variables")]
    public int DungeonPadding;
    public int DungeonRows, DungeonColumns;
    public int MinRoomSize, MaxRoomSize;

    [Header("Dungeon Tiles")]
    public GameObject[] BridgeTiles;
    public GameObject[] WallTiles;

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