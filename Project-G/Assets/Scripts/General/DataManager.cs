using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour
{
    /// <summary>Static reference to the instance of our DataManager</summary>
    private static DataManager _instance;
    public static DataManager Instance => _instance;

    [SerializeField] private int _dungeonLevel = 0;

    public int DungeonLevel
    {
        get => _dungeonLevel;
        set => _dungeonLevel = value;
    }

    [field: SerializeField] public int Health { get; set; } = 100;
    [field: SerializeField] public int WeaponID { get; set; } = 4;
    [field: SerializeField] public int Medkits { get; set; } = 1;
    [field: SerializeField] public int Shields { get; set; } = 1;
    
    private void Awake()
    {
        // If the instance reference has not been set, yet, 
        if (_instance == null)
        {
            // Set this instance as the instance reference.
            _instance = this;
        }
        else if (_instance != this)
        {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }

        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }

    public void ResetData()
    {
        DungeonLevel = 0;
        Health = 100;
        Medkits = 1;
        Shields = 1;
        WeaponID = 4;
    }
}