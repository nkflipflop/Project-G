using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour 
{
    /// <summary>Static reference to the instance of our DataManager</summary>
    private static DataManager _instance;

    [SerializeField] private int _dungeonLevel = 0, _medkits = 1, _shields = 1, _weaponID = 4;
    private int _health = 100;

    public static DataManager Instance { get { return _instance; } }

    public int DungeonLevel  {
        get { return _dungeonLevel; }
        set { _dungeonLevel = value; }
    }

    public int Health {
        get { return _health; }
        set { _health = value; }
    }

    public int WeaponID {
        get { return _weaponID; }
        set { _weaponID = value; }
    }

    public int Medkits  {
        get { return _medkits; }
        set { _medkits = value; }
    }

    public int Shields  {
        get {  return _shields; }
        set { _shields = value; }
    }

    /// <summary>Awake is called when the script instance is being loaded.</summary>
    private void Awake() {
        // If the instance reference has not been set, yet, 
        if (_instance == null) {
            // Set this instance as the instance reference.
            _instance = this;
        }
        else if (_instance != this) {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }
        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }

    public void ResetData() {
        DungeonLevel = 0;
        Health = 100;
        Medkits = 1;
        Shields = 1;
        WeaponID = 4;
    }
}