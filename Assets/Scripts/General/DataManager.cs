using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : Singleton<DataManager>
{
    [SerializeField] private int dungeonLevel;

    public int DungeonLevel
    {
        get => dungeonLevel;
        set => dungeonLevel = value;
    }

    [field: SerializeField] public int Health { get; set; } = 100;
    [field: SerializeField] public int WeaponID { get; set; } = 4;
    [field: SerializeField] public int MedKits { get; set; } = 1;
    [field: SerializeField] public int Shields { get; set; } = 1;

    public void ResetData()
    {
        DungeonLevel = 0;
        Health = 100;
        MedKits = 1;
        Shields = 1;
        WeaponID = 4;
    }
}