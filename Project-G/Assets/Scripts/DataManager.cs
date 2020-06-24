using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour 
{
    /// <summary>Static reference to the instance of our DataManager</summary>
    public static DataManager instance;

    private int _dungeonLevel = 0, _medkits = 3, _shields = 0;
    private float _health = 100;
    [SerializeField] private GameObject _weapon;        // initial weapon of the player

    public int DungeonLevel  {
        get { return _dungeonLevel; }
        set { _dungeonLevel = value; }
    }

    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    public GameObject Weapon {
        get { return _weapon; }
        set { _weapon = value; }
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
    void Awake() {
        // If the instance reference has not been set, yet, 
        if (instance == null) {
            // Set this instance as the instance reference.
            instance = this;
        }
        else if (instance != this) {
            // If the instance reference has already been set, and this is not the
            // the instance reference, destroy this game object.
            Destroy(gameObject);
        }

        // Do not destroy this object, when we load a new scene.
        DontDestroyOnLoad(gameObject);
    }
}